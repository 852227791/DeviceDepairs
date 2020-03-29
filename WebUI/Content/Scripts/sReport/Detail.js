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
        panelWidth: 300
    });
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        showFooter: true,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "DeptName", title: "收费单位", width: 200, sortable: true },
            { field: "Name", title: "收费项目", width: 150, sortable: true },
            { field: "PaidMoney", title: "实收金额", width: 120, sortable: true, halign: 'left', align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", width: 120, sortable: true, halign: 'left', align: 'right' },
            { field: "OffsetMoney", title: "充抵金额", width: 120, sortable: true, halign: 'left', align: 'right' },
            { field: "ByOffsetMoney", title: "被充抵金额", width: 120, sortable: true, halign: 'left', align: 'right' },
            { field: "RefundMoney", title: "核销金额", width: 120, sortable: true, halign: 'left', align: 'right' }
        ]],
        toolbar: Easy.loadToolbar(menuId, "1"),
        onLoadSuccess: function (data) {
            $('#grid').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
            var PaidMoney = 0;
            var DiscountMoney = 0;
            var OffsetMoney = 0;
            var ByOffsetMoney = 0;
            var RefundMoney = 0;
            for (var i = 0; i < data.rows.length; i++) {
                PaidMoney = parseFloat(PaidMoney) + parseFloat(data.rows[i].PaidMoney);
                DiscountMoney = parseFloat(DiscountMoney) + parseFloat(data.rows[i].DiscountMoney);
                OffsetMoney = parseFloat(OffsetMoney) + parseFloat(data.rows[i].OffsetMoney);
                ByOffsetMoney = parseFloat(ByOffsetMoney) + parseFloat(data.rows[i].ByOffsetMoney);
                RefundMoney = parseFloat(RefundMoney) + parseFloat(data.rows[i].RefundMoney);
            }
            $('#grid').datagrid('reloadFooter', [
               {
                   PaidMoney: '合计:' + PaidMoney.toFixed('2'), DiscountMoney: '合计:' + DiscountMoney.toFixed('2'),
                   OffsetMoney: '合计:' + parseFloat(OffsetMoney).toFixed('2'), ByOffsetMoney: '合计:' + parseFloat(ByOffsetMoney).toFixed('2'),
                   RefundMoney: '合计:' + parseFloat(RefundMoney).toFixed('2')
               },
            ]);
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        if (queryData.txtDeptID === "") {
            Easy.centerShow("系统消息", "请选择校区", 2000)
            return false;
        }
        $("#grid").datagrid({
            queryParams: queryData,//异步查询的参数
            url: "../sReport/GetDetailList",
        });
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        $("#grid").datagrid({ url: "", data: [] });
    });
}

//导出
download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        Easy.DeriveFileToGrid(".datagrid-view2", "学费明细汇总.xls");
    }
}

rerurnQueryData = function () {
    var data = {//得到用户输入的参数
        MenuID: menuId,
        txtDeptID: "" + $("#DeptID").combotree("getValue"),
        txtFeeTimeS: "" + $("#txtFeeTimeS").combobox("getValue"),
        txtFeeTimeE: "" + $("#txtFeeTimeE").combobox("getValue")
    }
    return data;
}