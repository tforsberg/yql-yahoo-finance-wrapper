using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESGI.Yahoo.Yql
{
    public class YQLQuery
    {
        private From_ _from;
        private String[] _select;

        public YQLQuery()
        {
            _from = new From_(); 
        }

        public String Yield()
        {
            return
                this.ToString()
                    .Replace(" ", "%20");
        }

        public override string ToString()
        {
            String result = "select ";

            foreach (String s in _select)
            {
                result+= s + ",";
            }

            result  =result.TrimEnd(',');

            result += " " + this._from.ToString();

            return result;
            
        }

        public From_ Select(String values)
        {
            String [] vals = values.Split(',');

            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = vals[i].Trim().ToLower();
            }
            _select = vals;

            return _from;
        }

        public From_ Select(String[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim().ToLower();
            }

            _select = values;

            return _from;
        }
    }

    public class From_
    {
        private Where_ _where;
        private String[] _table;


        public override string ToString()
        {
            String result = "from ";

            foreach (string s in _table)
            {
                result += s + ',';
            }

            result = result.TrimEnd(',');

            result += " "+ _where.ToString();

            return result;
        }

        public From_()
        {
            _where = new Where_();
        }

        public Where_ From(String table)
        {
            _table = new String[1];
            _table[0] = table.Trim().ToLower();

            return _where;
        }

        public Where_ From(String[] tables)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                tables[i] = tables[i].Trim().ToLower();
            }

            _table = new String[0];
            _table[0] = _table[0].Trim().ToLower();

            return _where;
        }
    }

    public class Where_
    {
        private List<KeyValuePair<String, String>> _where;
        private String[] _operators = new String[] { "in", "or", "and" };
        private String[] _tokens = new String[] { "(", ")","\"", "(",")","," };
        private String _criterias = string.Empty;

        public Where_()
        {
            _where = new List<KeyValuePair<String, String>>();
        }

        public override string ToString()
        {
            String result = string.Empty;

            if (string.Empty != _criterias)
            {
                result = "where "+ _criterias;
            }

            if (null != _where && _where.Count > 0)
            {
                result = "where ";

                int nb = 0;
                Boolean flag = false;

                foreach (KeyValuePair<String, String> kv in _where)
                {
                    nb++;

                    if (_operators.Contains(kv.Key) || _tokens.Contains(kv.Key))
                    {
                        if (kv.Key == "in")
                            flag = true;

                        if (kv.Key == ")")
                            flag = false;

                        result += (kv.Value == "\"") ? kv.Value : kv.Value + " ";
                    }
                    else 
                    {
                        if (flag || kv.Key == kv.Value)
                        {
                            
                            result += (kv.Value=="\"")?kv.Value: kv.Value +" ";
                        }
                        else
                        {
                            result += kv.Key + "=%22" + kv.Value + "%22 ";
                        }
                    }
                }
            }

            return result;
        }

        public Where_ Where()
        {
            return this;
        }

        public Where_ Where(String column, String value)
        {
            _criterias = string.Empty;
            _where.Add(new KeyValuePair<string, string>(column, value));
            return this;           
        }

        public Where_ Where(String criterias)
        {
            _criterias = criterias;
            _where = null;
            return this;
        }

        public Where_ And(String column, String value)
        {
            _criterias = string.Empty;
            _where.Add(new KeyValuePair<string, string>("and", "and"));
            _where.Add(new KeyValuePair<string, string>(column, value));
            return this;
        }

        public Where_ And(List<KeyValuePair<string, string>> values)
        {
            _criterias = string.Empty;
            _where.Add(new KeyValuePair<string, string>("and", "and"));
            _where.Add(new KeyValuePair<string, string>("(", "("));

            foreach (KeyValuePair<String, String> kv in values)
            {
                _where.Add(new KeyValuePair<string, string>(kv.Key, kv.Value));
            }

            _where.Add(new KeyValuePair<string, string>(")", ")"));

            return this;
        }

        public Where_ Or(List<KeyValuePair<string, string>> values)
        {
            _criterias = string.Empty;
            _where.Add(new KeyValuePair<string, string>("or", "or"));
            _where.Add(new KeyValuePair<string, string>("(", "("));
  

            foreach (KeyValuePair<String, String> kv in values)
            {
                _where.Add(new KeyValuePair<string, string>(kv.Key, kv.Value));
            }
      
            _where.Add(new KeyValuePair<string, string>(")", ")"));

            return this;
        }

        public Where_ Or(String column, String value)
        {
            _criterias = string.Empty;
            _where.Add(new KeyValuePair<string, string>("or", "or"));
            _where.Add(new KeyValuePair<string, string>(column, value));
            return this;
        }

        public Where_ In(String column, String values)
        {
            _criterias = string.Empty;

            _where.Add(new KeyValuePair<string, string>(column, column));
            _where.Add(new KeyValuePair<string, string>("in", "in"));
            _where.Add(new KeyValuePair<string, string>("(", "("));
            _where.Add(new KeyValuePair<string, string>("\"", "\""));

            foreach (String v in values.Split(','))
            {
                if (v.Trim() != String.Empty)
                {
                    _where.Add(new KeyValuePair<string, string>(column, v.Trim()));
                    _where.Add(new KeyValuePair<string, string>(",", ","));
                }
            }
            _where.RemoveAt(_where.Count - 1);
            _where.Add(new KeyValuePair<string, string>("\"", "\""));
            _where.Add(new KeyValuePair<string, string>(")", ")"));
            return this;
        }

        public Where_ In(String column, String[] values)
        {
            _criterias = string.Empty;
            _where.Add(new KeyValuePair<string, string>(column, column));
            _where.Add(new KeyValuePair<string, string>("in", "in"));
            _where.Add(new KeyValuePair<string, string>("(", "("));
            _where.Add(new KeyValuePair<string, string>("\"", "\""));
            int i = 0;

            foreach (String v in values)
            {
                i++;
                _where.Add(new KeyValuePair<string, string>(column, v));
                
                if(!(i==values.Count()))
                    _where.Add(new KeyValuePair<string, string>(",", ","));
            }

            _where.Add(new KeyValuePair<string, string>("\"", "\""));
            _where.Add(new KeyValuePair<string, string>(")", ")"));
         
            return this;
        }
    }
}
