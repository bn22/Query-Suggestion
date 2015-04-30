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

        public trieNode()
        {
            this.child = new trieNode[27];
            this.leaf = true;
            this.word = false;
        }

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