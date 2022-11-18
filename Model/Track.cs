using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {
        public string naam { get; set; }
        public LinkedList<Section> Sections;
        public Track(string naam, SectionTypes[] sections)
        {
            this.naam = naam;
            this.Sections = addLinkedList(sections);
        }
        public LinkedList<Section> addLinkedList(SectionTypes[] sections) {
            LinkedList<Section> list = new LinkedList<Section>();
            foreach (SectionTypes Sectiegedeelte in sections)
            {
                list.AddLast(new Section(Sectiegedeelte));
            }
            return list;
        }
        public Track() { 
        }
    }
}
