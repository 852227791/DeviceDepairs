var DeptID = "0";//定义ID
firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "DeptID", hidden: true },
            { field: "Name", title: "校区", width: 200, sortable: true },
            { field: "FileText", title: "数据文件", width: 140, sortable: true }
        ]],
        url: "../Prove/GetOldProveFileList", sortName: "ParentQueue", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1")
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({ MenuID: menuId });
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#treeDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
}

rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        treeDept: $("#treeDept").combotree("getValue")
    }
    return queryData;
}

//导出
derive = function () {
    //var queryData = rerurnQueryData();
    //$.ajax({
    //    type: "post",
    //    url: "../Prove/DownloadProveFile",
    //    async: false,
    //    dataType: "json",
    //    success: function (result) {
    //        if (result.IsError === false) {
    //            location.href = "../Temp/" + result.Message;
    //        }
    //    }
    //});
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "校区")) {
            var fileText = rows[0].FileText;
            if (fileText != "") {
                DeptID = rows[0].DeptID;
                location.href = "../Temp/" + rows[0].FileText;
            }
            else {
                Easy.centerShow("系统消息", "没有老系统数据", 3000);
            }
        }
    }
}
