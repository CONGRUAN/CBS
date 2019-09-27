using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.CBS.Model.Res
{
    #region 返回字段
    /// <summary>
    /// CBS打款返回信息
    /// </summary>
    public class CBSERPPGK
    {
        public INFO INFO { get; set; }
        /// <summary>
        /// 支付经办返回的
        /// </summary>
        public APPAYSAVZ APPAYSAVZ { get; set; }
        /// <summary>
        /// 查询支付业务是否存在返回的
        /// </summary>
        public ERPAYBUSZ ERPAYBUSZ { get; set; }
        public SYCOMRETZ SYCOMRETZ { get; set; }

        #region 扩展字段
        public bool IsSucess
        {
            get
            {
                if (this.INFO != null && this.INFO.RETCOD == "0000000" && this.APPAYSAVZ != null && this.APPAYSAVZ.ERRCOD == "0000000" && this.SYCOMRETZ != null && this.SYCOMRETZ.ERRCOD == "0000000")
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }

    public class INFO
    {
        public string ERPTYP { get; set; }
        public string ERRMSG { get; set; }
        public string FUNNAM { get; set; }
        public string RETCOD { get; set; }
    }

    public class APPAYSAVZ
    {
        public string BUSNBR { get; set; }
        public string ERRCOD { get; set; }
        public string REFNBR { get; set; }
        public string ERRMSG { get; set; }
    }

    public class SYCOMRETZ
    {
        public string ERRCOD { get; set; }
        public string ERRDTL { get; set; }
        public string ERRMSG { get; set; }
    }
    
    public class ERPAYBUSZ
    {
        public string BUSNBR { get; set; }
        public string ERRCOD { get; set; }
        public string REFNBR { get; set; }
    }
    #endregion
}
