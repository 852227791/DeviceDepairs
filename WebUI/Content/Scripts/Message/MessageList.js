firstFunction = function () {
    //加载表格
    initTable({});
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
            { field: "Id", hidden: true },
            { field: "Date", title: "Date", width: 120, sortable: true },
            { field: "Thread", title: "Thread", width: 100, sortable: true },
            { field: "Levels", title: "Levels", width: 60, sortable: true },
            {
                field: "Logger", title: "Logger", width: 200, sortable: true
            },
            {
                field: "Message", title: "Message", width: 460, sortable: true
            },
            {
                field: "Exception", title: "Exception", width: 300, sortable: true,
                formatter: function (value, row, index) {
                    if (value != "" && value != null) {
                        var str = "<span title=\"" + value+ "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                        return str;
                    }
                }
            }
        ]],
        url: "../Message/GetMessageList", sortName: "Id", sortOrder: "desc",
        onLoadSuccess: function (data) {
            Easy.bindCustomPromptToTableEvent(".tip");
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = {
            txtTimeS: $("#txtTimeS").datebox("getValue"),
            txtTimeE: $("#txtTimeE").datebox("getValue")
        }
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({  });
    });
}