using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.CBS.Model.Req
{
    /// <summary>
    /// 批量查询支付状态类
    /// </summary>
    public class PaySTAInfo
    {
        /// <summary>
        /// 客户业务参考号
        /// </summary>
        public string REFNBR { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string BUSNBR { get; set; }
    }
}
