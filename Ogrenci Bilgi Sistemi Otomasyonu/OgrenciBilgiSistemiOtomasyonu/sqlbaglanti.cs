using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SirketOtomasyonu
{
    internal class sqlbaglanti
    {
        public SqlConnection baglanti()
        {
            SqlConnection b =new SqlConnection("Data Source=DESKTOP-MACN9H5\\SQLEXPRESS;Initial Catalog=OkulOtomasyon;Integrated Security=True");
            b.Open();
            return b;

        }   
    }
}
