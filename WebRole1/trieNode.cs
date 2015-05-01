using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class trieNode
    {
        public trieNode[] child;
        public Boolean leaf;
        public Boolean word;
        public String value;

        /// <summary>
        /// This creates a new trieNode object that doesn't take into account any parameters
        /// </summary>
        public trieNode()
        {
            this.child = new trieNode[27];
            this.leaf = true;
            this.word = false;
        }

        /// <summary>
        /// This creates a trieNode object that accounts a character and a string as parameters
        /// </summary>
        /// <param name="character">char</param>
        /// <param name="value">String</param>
        public trieNode(char character, String value)
        {
            this.child = new trieNode[27];
            this.leaf = true;
            this.word = false;
            String newValue = value + character;
            this.value = newValue;
        }
    }
}