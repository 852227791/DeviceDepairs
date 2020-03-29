firstFunction = function () {
    bindSearchFormEvent();

    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索按钮的点击事件
    bindSearchClickEvent();
}

bindSearchFormEvent = function () {
    $("#DeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300,
        multiple: true,
        onlyLeafCheck: true
    });
}

//加载表格
initTable = function (queryData) {
    $("#grid").treegrid({
        striped: true,//斑马线
        singleSelect: true,//只允许选择一行
        showFooter: true,
        idField: 'DeptID',
        treeField: 'DeptName',
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "DeptName", title: "收费单位", width: 300, sortable: true },
            { field: "PaidMoney", title: "实交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, halign: 'left', align: 'right' },
            { field: "stu_Offset", title: "充抵金额(学费)", sortable: true, halign: 'left', align: 'right' },
            { field: "inc_Offset", title: "充抵金额(杂费)", sortable: true, halign: 'left', align: 'right' },
            { field: "_Offset", title: "充抵金额(证书费)", sortable: true, halign: 'left', align: 'right' },
            { field: "ByOffset", title: "被充抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' }
        ]],
        toolbar: Easy.loadToolbar(menuId, "1")
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        $("#grid").treegrid({
            queryParams: queryData,//异步查询的参数
            url: "../sReport/GetDeptList",
        });
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        $("#grid").treegrid({ url: "", data: [] });
    });
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        Easy.DeriveFileToGrid(".datagrid-view2", "学费收费报表.xls");
    }
}

rerurnQueryData = function () {
    var data = {//得到用户输入的参数
        MenuID: menuId,
        txtDeptID: "" + $("#DeptID").combotree("getValues"),
        txtFeeTimeS: "" + $("#txtFeeTimeS").combobox("getValue"),
        txtFeeTimeE: "" + $("#txtFeeTimeE").combobox("getValue")
    }
    return data;
}