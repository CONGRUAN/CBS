using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.CBS.Model.Req
{
    /// <summary>
    /// 查询支付业务是否存在类
    /// </summary>
    public class PayBUSInfo
    {
        /// <summary>
        /// 付方账号
        /// </summary>
        public string CLTACC { get; set; }
        /// <summary>
        /// 收款人账号
        /// </summary>
        public string REVACC { get; set; }
        /// <summary>
        /// 付款的金额
        /// </summary>
        public string TRSAMT { get; set; }
        /// <summary>
        /// 客户参考业务号
        /// </summary>
        public string REFNBR { get; set; }
    }
}
