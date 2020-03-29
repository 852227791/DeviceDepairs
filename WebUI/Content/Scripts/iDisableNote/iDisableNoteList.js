firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

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
            { field: "iDisableNoteID", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "Name", title: "作废人", width: 80, sortable: true },
            { field: "CreateTime", title: "作废时间", width: 130, sortable: true }
        ]],
        url: "../iDisableNote/GetiDisableNoteList", sortName: "iDisableNoteID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.Status === "2") {
                return "color:#ff0000;";
            }
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = {//得到用户输入的参数
            MenuID: menuId,
            txtVoucherNum: $("#txtVoucherNum").val(),
            txtNoteNum: $("#txtNoteNum").val()
        }
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({ MenuID: menuId });
    });
}