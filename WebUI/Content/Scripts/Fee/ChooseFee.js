
initChoosefeeTable1 = function (queryData) {
    $("#choosefeegrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "FeeDetailID", checkbox: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "Name", title: "姓名", width: 60, sortable: true },
            { field: "ItemName", title: "证书名称", width: 100, sortable: true },
            { field: "DetailName", title: "收费项目", width: 70, sortable: true },
            { field: "ShouldMoney", title: "应交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "实交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, halign: 'left', align: 'right' },
            { field: "OffsetMoney", title: "冲抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "BeOffsetMoney", title: "被冲抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "Dept", title: "校区", width: 140, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "FeeMode", title: "交费方式", width: 60, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "Affirm", title: "结账人", width: 60, sortable: true },
            { field: "AffirmTime", title: "结账时间", width: 80, sortable: true }
        ]],
        url: "../Fee/GetChooseFee", sortName: "FeeDetailID", sortOrder: "desc"
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearchChooseFee").click(function () {
        var queryData = rerurnQueryData();
        initChoosefeeTable1(queryData);//将值传递给
    });
    $("#btnResetChooseFee").click(function () {
        $("#fSearchChooseFee").form("reset");//重置表单
        initChoosefeeTable1({});
    });
}
rerurnQueryData = function () {
    var queryData = {
        voucherNum: $("#txtVoucherChooseFee").val(),
        studentName: $("#txtStudentNameChooseFee").val()
    }
    return queryData;
}

initChoosefeeTable1({});
bindSearchClickEvent();
