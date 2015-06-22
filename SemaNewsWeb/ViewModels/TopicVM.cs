using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace SemaNewsWeb.ViewModels
{
    public class TopicVM
    {
        public Topic Topic { get; set; }

        public List<string> Tags { get; set; }
        public List<string> KeyphrasesLDVL { get; set; }
        public List<string> KeyphrasesDT { get; set; }

        public TopicVM()
        {
            Topic = new Topic();
            Tags = new List<string>();
            KeyphrasesDT = new List<string>();
            KeyphrasesLDVL = new List<string>();

           
        }

        public TopicVM(Topic topic)
        {
            this.Topic = topic;
            this.Tags = new List<string>();
            this.KeyphrasesLDVL = new List<string>();
            this.KeyphrasesDT = new List<string>();

            if (this.Topic != null)
            {
                if (string.IsNullOrEmpty(topic.Tags) == false)
                    Tags = topic.Tags.Split(';').ToList();

                if (string.IsNullOrEmpty(topic.Keyphrases) == false)
                {
                    dynamic keyphrases = Json.Decode(topic.Keyphrases);
                    foreach (var keyphrase in keyphrases.LDVL)
                    {
                        this.KeyphrasesLDVL.Add(keyphrase);
                    }
                    foreach (var keyphrase in keyphrases.DT)
                        this.KeyphrasesDT.Add(keyphrase);
                }
            }
        }

        public Topic ParseToTopic()
        {
            this.Topic.Tags = string.Join(";", this.Tags);
            this.Topic.Keyphrases = Json.Encode(new { LDVL = this.KeyphrasesLDVL.ToArray(), DT = this.KeyphrasesDT.ToArray() });
            return this.Topic;
        }
    }
}