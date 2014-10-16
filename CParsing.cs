using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MyCompilation
{
    class CParsing
    {
        //用于存储非终结符
        public Dictionary<string, CNTChar> ntChars 
            = new Dictionary<string,CNTChar>();
        //用于存储终结符
        public Dictionary<string, CTChar> tChars 
            = new Dictionary<string, CTChar>();
        //用于存储产生式
        public  ArrayList productions = new ArrayList();
        //用于存储预测分析表
        public Dictionary<string, Dictionary<string, ArrayList>> predictionTable
            = new Dictionary<string, Dictionary<string, ArrayList>>();

        //用于存储可退出空串的终结符
        public ArrayList produceNullChars = new ArrayList();

        public ArrayList tokens = new ArrayList();

        public static bool firstChanged;

        //对产生式进行分析将字符分类加到对应的列表中
        public void charTNT()
        {
            //左部非终结符
            foreach (Production pro in productions)
            {
                if (!ntChars.ContainsKey(pro.Left))
                {
                    ntChars.Add(pro.Left, new CNTChar(pro.Left));
                }
            }
            //找出右部中的终结符
            foreach (Production pro in productions)
            {
                ArrayList tmpRight = new ArrayList();
                foreach (string temp in pro.Right)
                {
                    tmpRight.Add(temp);
                }
                foreach (string key in ntChars.Keys)
                {
                    tmpRight.Remove(key);
                    tmpRight.Remove(key);
                    tmpRight.Remove(key);
                    tmpRight.Remove(key);
                    tmpRight.Remove(key);
                }
                foreach (string s1 in tmpRight)
                {
                    if (!tChars.ContainsKey(s1))
                    {
                        tChars.Add(s1,new CTChar(s1));
                    }
                }

            }
        }

        //设置终结符FIRST集
        public void setTCharFirst(string X)
        {
            tChars[X].First.Add(X);
        }

        //初始化非终结符的FIRST集
        public void initNTCharFirst(string X)
        {
            foreach (Production p in productions)
            {
                if (X.Equals(p.Left))
                {
                    string firstR = (string)(p.Right)[0];
                    //右面第一个是终结符
                    if (isTChar(firstR))
                    {
                        if (!ntChars[X].First.Contains(firstR))
                        {
                            ntChars[X].First.Add(firstR);
                        }
                    }
                }
            }
        }

        //对非终结符的First集进行计算
        public void addNTCharFirst(string X)
        {
            foreach (Production p in productions)
            {
                //左部匹配
                if (X.Equals(p.Left))
                {
                    //右部第一个字符
                    string firstR = (string)p.Right[0];
                    //非终结符
                    if (isNTChar(firstR))
                    {
                        //将其FIRST集并入
                        foreach (string s in ntChars[firstR].First)
                        {
                            if (!ntChars[X].First.Contains(s))
                            {
                                ntChars[X].First.Add(s);
                                firstChanged = true;
                            }
                        }
                    }
                }
            }

            foreach (Production p in productions)
            {
                if (X.Equals(p.Left))
                {
                    string current = "";
                    //右侧的所有字符
                    foreach (string s0 in p.Right)
                    {
                        current = s0;
                        //可推导出空串
                        if (canLeadNull(current))
                        {
                            //将其FIRST集加入
                            foreach (string s in getFirst(current))
                            {
                                if (!ntChars[X].First.Contains(s))
                                {
                                    ntChars[X].First.Add(s);
                                    firstChanged = true;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach(string s in getFirst(current))
                    {
                        if(!ntChars[X].First.Contains(s))
                        {
                            ntChars[X].First.Add(s);
                            firstChanged=true;

                        }
                    }
                }
            }
        }

        //是否为终结符
        public bool isTChar(string s)
        {
            return tChars.ContainsKey(s);
        }

        //是否为非终结符
        public bool isNTChar(string s)
        {
            return ntChars.ContainsKey(s);
        }

        //是否可以退出空串
        public bool canLeadNull(string X)
        {
            if (isTChar(X))
                return false;
            else
            {
                if (produceNullChars.Contains(X))
                    return true;

                foreach (Production p in productions)
                {
                    if (X.Equals(p.Left))
                    {
                        //当前存在$
                        if ("$".Equals(p.Right[0]))
                        {
                            produceNullChars.Add(X);
                            return true;
                        }
                        //递归查找
                        else
                        {
                            bool flag = true;
                            foreach (string s in p.Right)
                            {
                                if (!canLeadNull(s))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag == true)
                            {
                                produceNullChars.Add(X);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        //获得字符串的FIRST集
        public ArrayList getFirst(string X)
        {
            if (isNTChar(X))
            {
                return ntChars[X].First;
            }
            else if (isTChar(X))
            {
                return tChars[X].First;
            }
            else
                return null;
        }

    }
}
