var NoteID = "0";//定义ID
firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });
    bindSearchFormEvent();
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
            { field: "NoteID", hidden: true },
            { field: "Dept", title: "校区", width: 200, sortable: true },
            { field: "SortName", title: "分类", width: 100, sortable: true },
            { field: "InFile", title: "导入文件路径", width: 300, sortable: true },
            { field: "OutFile", title: "错误文件路径", width: 280, sortable: true },
            { field: "SuccessNum", title: "成功条数", sortable: true, halign: 'left', align: 'right' },
            { field: "ErrorNum", title: "错误条数", sortable: true, halign: 'left', align: 'right' },
            { field: "CreateTime", title: "导入时间", width: 150, sortable: true },
        ]],
        url: "../Note/GetNoteList", sortName: "NoteID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
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
rerurnQueryData = function () {
    var queryData = {
        MenuID: menuId,
        selDeptID: $("#txtDept").combotree("getValue"),
        txtCreateTime: $("#txtCreateTime").datebox("getValue")
    }
    return queryData;
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#txtDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 200
    });
}

deriveUp = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveUp")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "导入记录信息")) {
            location.href = rows[0].InFile;
        }
    }
}
deriveDown = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveDown")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "导入记录信息")) {
            if (rows[0].OutFile === "" || rows[0].OutFile === null) {
                Easy.centerShow("系统消息", "不存在错误文档", 3000);

            } else {
                location.href = rows[0].OutFile;
            }
        }
    }
}

