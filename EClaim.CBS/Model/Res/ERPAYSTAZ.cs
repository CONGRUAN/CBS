using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.CBS.Model.Res
{
    public class ERPAYSTAZ
    {
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string BUSNBR { get; set; }
        /// <summary>
        /// 付款账号
        /// </summary>
        public string CLTACC { get; set; }
        /// <summary>
        /// 错误码
        /// 0000000 表示成功
        /// </summary>
        public string ERRCOD { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string OPRTYP { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public string OPTSTU { get; set; }
        /// <summary>
        /// 客户参考业务号
        /// </summary>
        public string REFNBR { get; set; }
        /// <summary>
        /// 记录状态
        /// 0查无此记录（状态可疑）  1：支付成功  2：支付失败  3：未完成   4：银行退票
        /// </summary>
        public string STATUS { get; set; }
        /// <summary>
        /// 支付失败或否决时的错误信息
        /// </summary>
        public string REMARK { get; set; }
    }
}
