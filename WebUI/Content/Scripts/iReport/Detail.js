var columnsStr = null;
var isParent = "No";
firstFunction = function () {
    bindSearchFormEvent();

    initColumns({ MenuID: menuId });
    //加载表格
    initTable({ MenuID: menuId, isParent: isParent });

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
    $("#FeeTimeS").datebox({});
    $("#FeeTimeE").datebox({});
}

initColumns = function (queryData) {
    $.ajax({
        type: "post",
        url: "../iReport/GetColumns",
        async: false,
        data: queryData,
        dataType: "json",
        success: function (result) {
            columnsStr = result.Message;
        },
        error: function () {
            Easy.centerShow("系统消息", "发现系统错误", 3000);
        }
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
        columns: eval(columnsStr),//eval(columnsStr)
        frozenColumns: [[
            { field: "DeptName", title: "单位", halign: 'left', align: 'left' }
        ]],
        toolbar: Easy.loadToolbar(menuId, "1"),
        onLoadSuccess: function (data) {
            $('#grid').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initColumns(queryData);
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initColumns({ MenuID: menuId });
        initTable({ MenuID: menuId, isParent: isParent });
    });
}

changeParent = function () {
    isParent = "Yes";
    var queryData = rerurnQueryData();
    $("#grid").treegrid({
        queryParams: queryData,//异步查询的参数
        url: "../iReport/GetDetailList",
    });
}

changeDept = function () {
    isParent = "No";
    var queryData = rerurnQueryData();
    $("#grid").treegrid({
        queryParams: queryData,//异步查询的参数
        url: "../iReport/GetDetailList",
    });
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        Easy.DeriveFileToGrid(".datagrid-view2", "杂费类别报表.xls");
    }
}

rerurnQueryData = function () {
    var data = {//得到用户输入的参数
        MenuID: menuId,
        txtDeptID: "" + $("#DeptID").combotree("getValue"),
        txtFeeTimeS: "" + $("#txtFeeTimeS").combobox("getValue"),
        txtFeeTimeE: "" + $("#txtFeeTimeE").combobox("getValue"),
        isParent: isParent
    }
    return data;
}