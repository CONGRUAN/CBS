using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.CBS.Model.Req
{
    /// <summary>
    /// 支付经办请求类
    /// 有默认值的字段可不赋值
    /// </summary>
    public class PaySAVInfo
    {
        /// <summary>
        /// 收款银行类型
        /// CMB-招商银行
        /// 按需要参照文档
        /// 对外支付、银行调拨、集中支付不能为空
        /// </summary>
        public string BNKTYP { get; set; } = "CMB";
        /// <summary>
        /// 业务子类型
        /// 0-标准支付
        /// </summary>
        public string BUSTYP { get; set; } = "0";
        /// <summary>
        /// 币种
        /// 按需要参照文档
        /// </summary>
        public string CCYNBR { get; set; } = "10";
        /// <summary>
        /// 结算方式
        /// 0-其它    2-转账    3-电汇    4-汇票
        /// </summary>
        public string PAYTYP { get; set; } = "2";
        /// <summary>
        /// 付方账号
        /// </summary>
        public string CLTACC { get; set; }
        /// <summary>
        /// 付方客户号
        /// </summary>
        public string CLTNBR { get; set; }
        /// <summary>
        /// 期望日（大于等于当前交易日期），直连提交银行的付款日期
        /// </summary>
        public string EPTDAT { get; set; }
        /// <summary>
        /// 期望时间
        /// </summary>
        public string EPTTIM { get; set; }
        /// <summary>
        /// 摘要
        /// 可为空
        /// </summary>
        public string EXTTX1 { get; set; } = "";
        /// <summary>
        /// 支付渠道
        /// 0-其他    2-打印银行票据    3-银企直联支付    5-银保通支付
        /// </summary>
        public string OPRMOD { get; set; } = "3";
        /// <summary>
        /// 操作类型
        /// 202-对外支付    204-银行调拨    206-内转      401-集中支付    235-网银互联    236-集中网银互联
        /// </summary>
        public string OPRTYP { get; set; } = "202";
        /// <summary>
        /// 客户参考业务号
        /// ERP系统唯一编号，此编号同一渠道不能重复提交（该编号不止是指同批次不能重复，任何情况都不能重复，银行有存档）
        /// </summary>
        public string REFNBR { get; set; }
        /// <summary>
        /// 收款人账号
        /// </summary>
        public string REVACC { get; set; }
        /// <summary>
        /// 收款人开户行
        /// </summary>
        public string REVBNK { get; set; }
        /// <summary>
        /// 收方城市（文档要求不能为空，经测试可以为空）
        /// 对外支付、银行调拨、集中支付不能为空
        /// </summary>
        public string REVCIT { get; set; }
        /// <summary>
        /// email地址
        /// 可空
        /// </summary>
        public string REVEML { get; set; }
        /// <summary>
        /// 移动电话
        /// 可空
        /// </summary>
        public string REVMOB { get; set; }
        /// <summary>
        /// 收款人姓名
        /// 对外支付、银行调拨、集中支付不能为空
        /// </summary>
        public string REVNAM { get; set; }
        /// <summary>
        /// 收方省份（文档要求不能为空，经测试可以为空）
        /// 对外支付、银行调拨、集中支付不能为空
        /// </summary>
        public string REVPRV { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal TRSAMT { get; set; }
        /// <summary>
        /// 交易用途
        /// 不能为空
        /// </summary>
        public string TRSUSE { get; set; }
    }
}
