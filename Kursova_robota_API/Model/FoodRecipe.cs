using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursova_robota_API.Model
{
    public class Results
    {
        public List<ResultItem> results { get; set; }
    }
    public class ResultItem
    {
        public string name { get; set; }
        public string description { get; set; }
        public string original_video_url { get; set; }
        public List<InstructionItem> instructions { get; set; }
        public List<TagItem> tags { get; set; }
    }
    public class TagItem
    {
        public string display_name { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }
    public class InstructionItem
    {
        public int position { get; set; }
        public string display_text { get; set; }
    }



    /*public class Recipe
    {
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string dietId { get; set; }
        public string dietDescription { get; set; }
        public string cuisineId { get; set; }
        public string cuisineDescription { get; set; }
    }


    public class TagList
    {
        public EnUs en_Us { get; set; }
    }
        public class EnUs
    {
        public List<UserDietItem> user_diet { get; set; }
        public List<CuisineItem> cuisine { get; set; }
    }
    public class UserDietItem
    {
        public string id { get; set; }
        public string description { get; set; }
    }
    public class CuisineItem
    {
        public string id { get; set; }
        public string description { get; set; }
    }


    public class FeedsList
    {
        public List<FeedItem> feed { get; set; }
    }
    public class FeedItem
    {
        public Seo seo { get; set; }
    }
    public class Seo
    {
        public Firebase firebase { get; set; }
    }
    public class Firebase
    {
        public string webUrl { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }*/
}
