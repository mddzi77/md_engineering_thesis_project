using System.Collections.Generic;
using Game.Transistors;
using UnityEngine;

namespace Game.Checker
{
    public class Node
    {
        public string ID { get; private set; }
        public List<GameObject> Cells = new();
        public List<GameObject> Contacts = new();
        public List<PTransistor> PTransistors = new();
        public List<NTransistor> NTransistors = new();

        public Node(string id)
        {
            ID = id;
        }

        public void Restore()
        {
            Contacts.ForEach(contact => contact.SetActive(true));
            Cells.ForEach(cell => cell.SetActive(true));
        }
        
        public void ChangeID(string id)
        {
            ID = id;
        }
    }
}