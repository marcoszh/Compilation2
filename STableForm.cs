using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace MyCompilation
{
    public partial class STableForm : Form
    {
        public ArrayList tokens = new ArrayList();
        public ArrayList productions = new ArrayList();
        public STableForm()
        {
            InitializeComponent();
            updateTable();
        }

        /*public void process()
        {
            mParsing.productions = CSUtility.readProductionFile("Grammar.txt");

            mParsing.charTNT();

            foreach (string s in mParsing.tChars.Keys)
            {
                mParsing.setTCharFirst(s);
            }

            foreach (string s in mParsing.ntChars.Keys)
            {
                mParsing.initNTCharFirst(s);
            }

            while (mParsing.isChanged == true)
            {
                mParsing.isChanged = false;
                foreach (string s in mParsing.ntChars.Keys)
                {
                    mParsing.addNTCharFirst(s);
                }
            }

            //去掉FIRST中的空串
            foreach (string s in mParsing.ntChars.Keys)
            {
                mParsing.ntChars[s].First.Remove("$");
            }

            mParsing.initFollow();
            mParsing.isChanged = true;
            while (mParsing.isChanged == true)
            {
                mParsing.isChanged = false;
                mParsing.addNTFollow();
            }

            mParsing.setSelectForProduction();

            mParsing.setPredictionTable();

            mParsing.setSync();
        }*/

        public void updateTable()
        {
            foreach (Production production in productions)
            {
                foreach(string s in production.Right)
                {
                    ListViewItem productionItem = new ListViewItem(production.Left);
                    productionItem.SubItems.Add(production.no.ToString());
                    productionItem.SubItems.Add(production.Left);
                    productionItem.SubItems.Add(s);
                    string selectStr="";
                    foreach (string s1 in production.Select)
                        selectStr += (s1+" ");
                    productionItem.SubItems.Add(selectStr);
                    predictionListView.Items.Add(productionItem);
                }
            }

        }
    }
}
