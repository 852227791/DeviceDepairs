//查询条件数据
rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtStudentNameChooseFee: $("#txtStudentNameChooseFee").textbox("getValue"),
        txtVoucherChooseFee: $("#txtVoucherChooseFee").textbox("getValue")
    }
    return queryData;
}

//绑定表单数据
initChooseiFeeGrid = function (queryData) {
    ifee = $("#choosefeegrid").datagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "iFeeID", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 110, sortable: true },
            { field: "Dept", title: "收费单位", width: 120, sortable: true },
            { field: "Name", title: "人员姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "FeeContent", title: "收费项目", width: 60, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "CanMoney", title: "可核销金额", width: 70, sortable: true, align: 'right' }
        ]],
        url: "../iFee/GetChooseiFeeList", sortName: "FeeTime", sortOrder: "desc"
    });
}

//绑定搜索按钮的点击事件
bindChooseSearchClickEvent = function () {
    $("#btnSearchChooseFee").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initChooseiFeeGrid(queryData);//将值传递给
    });
    $("#btnResetChooseFee").click(function () {
        $("#fSearchChooseFee").form("reset");//重置表单
        initChooseiFeeGrid({ MenuID: menuId });
    });
}

//加载表单数据
initChooseiFeeGrid({ MenuID: menuId });

//加载搜索按钮的点击事件
bindChooseSearchClickEvent();

