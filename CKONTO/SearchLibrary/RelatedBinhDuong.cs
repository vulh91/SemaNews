using SearchLibrary.DatabaseModel;
using SearchLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class RelatedBinhDuong
    {
        static CKONTOLOGYEntities dbCKOntology;

        static List<Location> location = new List<Location>();
        static List<Organization> organization = new List<Organization>();
        static List<Person> person = new List<Person>();

        public static void InitialData()
        {
            using (dbCKOntology = new CKONTOLOGYEntities())
            {
                location = dbCKOntology.Locations.Select(a => a).ToList();
                organization = dbCKOntology.Organizations.Select(a => a).ToList();
                person = dbCKOntology.People.Select(a => a).ToList();
            }
        }

        public static List<Location> locationList
        {
            get
            {
                return location;
            }
        }
        public static List<Organization> organizationList
        {
            get
            {
                return organization;
            }
        }
        public static List<Person> personList
        {
            get
            {
                return person;
            }
        }

        /// <summary>
        /// Ham tinh hang cua 1 tin bai co lien quan toi tinh Binh Duong
        /// </summary>
        /// Cac tham so da duoc Normalize (tuc da duoc chuan hoa o dang chuan)
        /// <param name="_articleId"></param>
        /// <param name="_title"></param>
        /// <param name="_abstract"></param>
        /// <param name="_content"></param>
        /// <param name="_tags"></param>
        /// <returns>
        /// tra ve hang cua 1 tin bai co lien quan toi tinh Binh Duong
        /// </returns>
        public static double GetRank(string _title, string _abstract, string _content, string _tags)
        {
            double rank = 0;
            long number = 0;
            _title = HTMLHelper.NormalizeStr4Search(_title, false);
            _abstract = HTMLHelper.NormalizeStr4Search(_abstract, false);
            _content = HTMLHelper.NormalizeStr4Search(_content, false);
            _tags = HTMLHelper.NormalizeStr4Search(_tags, false);
            foreach (Location aLocation in locationList)
            {
                float weight = 0;
                string locationName = HTMLHelper.NormalizeStr4Search(aLocation.LocationName, false);
                weight = (aLocation.LocationWeight != null ? (float)aLocation.LocationWeight : 0);
                if (Regex.Matches(_title, locationName).Count > 0)
                {
                    number += Regex.Matches(_title, locationName).Count;
                    rank += Regex.Matches(_title, locationName).Count * weight;
                }
                if (Regex.Matches(_abstract, locationName).Count > 0)
                {
                    number += Regex.Matches(_abstract, locationName).Count;
                    rank += Regex.Matches(_abstract, locationName).Count * weight;
                }
                if (Regex.Matches(_tags, locationName).Count > 0)
                {
                    number += Regex.Matches(_tags, locationName).Count;
                    rank += Regex.Matches(_tags, locationName).Count * weight;
                }
                if (Regex.Matches(_content, locationName).Count > 0)
                {
                    number += Regex.Matches(_content, locationName).Count;
                    rank += Regex.Matches(_content, locationName).Count * weight;
                }
            }
            foreach (Organization aOrganization in organizationList)
            {
                float weight = 0;
                string organizationName = HTMLHelper.NormalizeStr4Search(aOrganization.OrganizationName, false);
                string synonymName = HTMLHelper.NormalizeStr4Search(aOrganization.Synonym, false);
                weight = (aOrganization.OrganizationWeight != null ? (float)aOrganization.OrganizationWeight : 0);
                if (!string.IsNullOrEmpty(organizationName))
                {
                    if (Regex.Matches(_title, organizationName).Count > 0)
                    {
                        number += Regex.Matches(_title, organizationName).Count;
                        rank += Regex.Matches(_title, organizationName).Count * weight;
                    }
                    if (Regex.Matches(_abstract, organizationName).Count > 0)
                    {
                        number += Regex.Matches(_abstract, organizationName).Count;
                        rank += Regex.Matches(_abstract, organizationName).Count * weight;
                    }
                    if (Regex.Matches(_tags, organizationName).Count > 0)
                    {
                        number += Regex.Matches(_tags, organizationName).Count;
                        rank += Regex.Matches(_tags, organizationName).Count * weight;
                    }
                    if (Regex.Matches(_content, organizationName).Count > 0)
                    {
                        number += Regex.Matches(_content, organizationName).Count;
                        rank += Regex.Matches(_content, organizationName).Count * weight;
                    }
                }
                if (!string.IsNullOrEmpty(synonymName))
                {
                    if (Regex.Matches(_title, synonymName).Count>0)
                    {
                        number+=Regex.Matches(_title, synonymName).Count;
                        rank += Regex.Matches(_title, synonymName).Count * weight;
                    }
                    if (Regex.Matches(_abstract, synonymName).Count >0)
                    {
                        number+=Regex.Matches(_abstract, synonymName).Count;
                        rank += Regex.Matches(_abstract, synonymName).Count * weight;
                    }
                    if (Regex.Matches(_tags, synonymName).Count > 0)
                    {
                        number+=Regex.Matches(_tags, synonymName).Count;
                        rank += Regex.Matches(_tags, synonymName).Count * weight;
                    }
                    if (Regex.Matches(_content, synonymName).Count > 0)
                    {
                        number += Regex.Matches(_content, synonymName).Count;
                        rank += Regex.Matches(_content, synonymName).Count * weight;
                    }
                }
            }
            foreach (Person person in personList)
            {
                float weight = 0;
                string personName = HTMLHelper.NormalizeStr4Search(person.PersonName, false);
                string identity = HTMLHelper.NormalizeStr4Search(person.IdentityString, false);
                string synonymidentity = HTMLHelper.NormalizeStr4Search(person.IdentitySynonym, false);
                weight = (person.PersonWeight != null ? (float)person.PersonWeight : 0);
                if (!string.IsNullOrEmpty(identity))
                {
                    if (Regex.Matches(_title, identity).Count > 0)
                    {
                        number += Regex.Matches(_title, personName).Count;
                        rank += Regex.Matches(_title, personName).Count * weight;
                    }
                    if (Regex.Matches(_abstract, identity).Count > 0)
                    {
                        number += Regex.Matches(_abstract, personName).Count;
                        rank += Regex.Matches(_abstract, personName).Count * weight;
                    }
                    if (Regex.Matches(_tags, identity).Count > 0)
                    {
                        number += Regex.Matches(_tags, personName).Count;
                        rank += Regex.Matches(_tags, personName).Count * weight;
                    }
                    if (Regex.Matches(_content, identity).Count > 0)
                    {
                        number += Regex.Matches(_content, personName).Count;
                        rank += Regex.Matches(_content, personName).Count * weight;
                    }
                }
                else if (!string.IsNullOrEmpty(synonymidentity))
                {
                    if (Regex.Matches(_title, synonymidentity).Count > 0)
                    {
                        number += Regex.Matches(_title, personName).Count;
                        rank += Regex.Matches(_title, personName).Count * weight;
                    }
                    if (Regex.Matches(_abstract, synonymidentity).Count > 0)
                    {
                        number += Regex.Matches(_abstract, personName).Count;
                        rank += Regex.Matches(_abstract, personName).Count * weight;
                    }
                    if (Regex.Matches(_tags, synonymidentity).Count > 0)
                    {
                        number += Regex.Matches(_tags, personName).Count;
                        rank += Regex.Matches(_tags, personName).Count * weight;
                    }
                    if (Regex.Matches(_content, synonymidentity).Count > 0)
                    {
                        number += Regex.Matches(_content, personName).Count;
                        rank += Regex.Matches(_content, personName).Count * weight;
                    }
                }
            }
            rank = rank / number;
            return rank;
        }
    }
}
