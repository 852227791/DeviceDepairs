var selectRow;

initFunction = function () {
    selectRow = $("#grid").datagrid("getSelected");
}

bindiFeeInfo = function () {
    $("#spDeptName").html(selectRow.Dept);
    $("#spFeeDept").html(selectRow.DeptAreaName);
    $("#spPersonName").html(selectRow.Name);
    $("#spItemName").html(selectRow.FeeContent);
    $("#spPersonType").html(selectRow.PersonSort);
    $("#spFeeType").html(selectRow.FeeMode);
    $("#spVocNum").html(selectRow.VoucherNum);
    $("#spBillNum").html(selectRow.NoteNum);
    $("#spShouldMoney").html(selectRow.ShouldMoney);
    $("#spPaidMoney").html(selectRow.PaidMoney);
    $("#spDiscountMoney").html(selectRow.DiscountMoney);
    $("#spOffsetMoney").html(selectRow.OffsetMoney);
    $("#spByOffsetMoney").html(selectRow.BeOffsetMoney);
    $("#spRefundMoney").html(selectRow.RefundMoney);
    $("#spFeeUser").html(selectRow.CreateName);
    $("#spFeeTime").html(selectRow.FeeTime);
    $("#spAffirmUser").html(selectRow.Affirm);
    $("#spAffirmTime").html(selectRow.AffirmTime);
    $("#spExplain").html(selectRow.Explain);
    $("#spRemark").html(selectRow.Remark);
    $("#spStatus").html(selectRow.Status);
}


tabsLoad = function (i) {
    $('#ViewInfo').tabs({
        border: false,
        selected: i,
        onSelect: function (title, index) {
            if (index == 0) {
                bindiFeeInfo();
            }
            else if (index === 1) {
                Easy.bindSelectStudentInfo(selectRow.StudentID);
            }
            else if (index === 2) {
                ifeeRefund();
            }
        }
    });
}
ifeeRefund = function () {
    $("#refundGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        queryParams: { iFeeID: selectRow.iFeeID },//异步查询的参数
        columns: [[
            { field: "iRefundID", hidden: true },
             { field: "DeptName", title: "收费单位", width: 120, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "FeeContent", title: "收费项", width: 100, sortable: true },
            { field: "Sort", title: "核销类别", width: 80, sortable: true },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundTime", title: "核销时间", width: 80, sortable: true },
            { field: "PayObject", title: "支付对象", width: 80, sortable: true },
            { field: "CreateName", title: "核销员", sortable: true, width: 80 },
            { field: "Remark", title: "核销备注", sortable: true, width: 200 }
        ]],
        url: "../iRefund/GetViewiRefundList", sortName: "iRefundID", sortOrder: "desc",
        rowStyler: function (index, row) {
            if (row.Status === "2") {
                return "color:#ff0000;";
            }
        }
    });
}

initFunction();

tabsLoad(0);