var StudentID = "0";
bindSelectProveInfo = function () {
    var result = Easy.bindSelectInfo("../Prove/SelectViewProve", ProveID);
    $("#spProfession").html(result.Data[0].ProfessionName);
    $("#spClass").html(result.Data[0].ClassName);
    $("#spItemName").html(result.Data[0].Name);
    $("#spEnrollTime").html(result.Data[0].EnrollTime);
    $("#spStatus").html(result.Data[0].Status);
    $("#spForce").html(result.Data[0].Force);
    $("#spItemRemark").html(result.Data[0].Remark); 
    $("#spDeptName").html(result.Data[0].DeptName);
    $("#spProveNum").html(result.Data[0].ProveNum);
    StudentID = result.Data[0].StudentID;
}



initviewTable = function (queryData) {
    $("#feeGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "FeeID", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "Dept", title: "校区", width: 140, sortable: true },
            { field: "Name", title: "学生姓名", width: 60, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "ItemName", title: "证书名称", width: 100, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "FeeMode", title: "交费方式", width: 60, sortable: true },
            { field: "ShouldMoney", title: "应交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "实交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, halign: 'left', align: 'right' },
            { field: "OffsetMoney", title: "冲抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "BeOffsetMoney", title: "被冲抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "Affirm", title: "结账人", width: 60, sortable: true },
            { field: "AffirmTime", title: "结账时间", width: 80, sortable: true }
        ]],
        url: "../Fee/GetFeeList", sortName: "FeeID", sortOrder: "desc",
        rowStyler: function (index, row) {
            if (row.StatusValue === "9") {
                return "color:#ff0000;";
            }
            else if (row.StatusValue === "2") {
                return "color:#339900;";
            }
        }
    });
}

tabsLoad = function (i) {
    $('#ViewInfo').tabs({
        border: false,
        selected: i,
        onSelect: function (title, index) {
            if (index === 0) {
                bindSelectProveInfo();
            }
            else if (index === 1) {
                Easy.bindSelectStudentInfo(StudentID);
            }
            else if (index === 2) {
                initviewTable({ MenuID: "16", txtProveID: ProveID });
            }
        }
    });
}
tabsLoad(0);