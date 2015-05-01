using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class trie
    {
        private trieNode root;
        public trie()
        {
            this.root = new trieNode();
        }
        public void addTitles(String title)
        {
            title = title.ToLower();
            root = addTitlesHelper(title, root);
        }
        private trieNode addTitlesHelper(String title, trieNode root)
        {
            int pos = title[0] - 'a';
            if (pos == -2)
            {
                pos = 26;
            }
            if (root.child[pos] == null)
            {
                if (title[0] == 95)
                {
                    root.child[pos] = new trieNode(' ', root.value);
                }
                else
                {
                    root.child[pos] = new trieNode(title[0], root.value);
                }
            }
            if (title.Length > 1)
            {
                root.child[pos] = addTitlesHelper(title.Substring(1), root.child[pos]);
            }
            else //title.Length <= 1
            {
                root.child[pos].word = true;
            }
            return root;
        }

        public List<String> findSuggestions(String prefix)
        {
            prefix = prefix.ToLower();
            List<String> results = new List<String>();
            Boolean allAlphabet = true;
            for (int i = 0; i < prefix.Length; i++)
            {
                if (prefix[i] < 65 || prefix[i] > 90)
                {
                    if (prefix[i] < 97 || prefix[i] > 122)
                    {
                        if (prefix[i] != 32)
                        {
                            allAlphabet = false;
                        }
                    }
                }
                if (allAlphabet == false)
                {
                    return results;
                }
            }
            return findSuggestionsHelper(prefix, root, results);
        }

        private List<String> findSuggestionsHelper(String prefix, trieNode root, List<String> results)
        {
            trieNode lastLetterOfPrefix = new trieNode();
            if (root == null)
            {
                return results;
            }
            else //root != null
            {
                for (int i = 0; i < prefix.Length; i++)
                {
                    int pos = prefix[i] - 'a';
                    if (pos == -2 || pos == -65)
                    {
                        pos = 26;
                    }
                    try
                    {
                        root = root.child[pos];
                    }
                    catch (NullReferenceException)
                    {
                    }
                    lastLetterOfPrefix = root;
                }
                for (int i = 0; i < 27; i++)
                {
                    try
                    {
                        listPopulator(root.child[i], results);
                    }
                    catch (NullReferenceException)
                    {

                    }
                }
            }
            return results;
        }

        private void listPopulator(trieNode root, List<String> results)
        {
            if (root != null && results.Count() <= 10)
            {
                if (root.word == true)
                {
                    results.Add(root.value);
                }
                for (int i = 0; i < 27; i++)
                {
                    listPopulator(root.child[i], results);
                }
            }
        }
    }
}