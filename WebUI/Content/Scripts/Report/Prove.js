var clearStr = "No";
var buttonStr = null;
firstFunction = function () {
    bindSearchFormEvent();

    buttonStr = Easy.loadToolbar(menuId, "1");

    //加载表格
    initTable({ Clear: clearStr, MenuID: menuId });

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

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "DeptName", title: "校区", width: 180, sortable: true },
            { field: "ParentName", title: "证书分类", width: 80, sortable: true },
            { field: "Name", title: "证书名称", width: 180, sortable: true },
            { field: "Num", title: "报考人次", sortable: true, halign: 'left', align: 'right' },
            { field: "ShouldMoney", title: "应交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "实交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, halign: 'left', align: 'right' },
            { field: "Offset", title: "充抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "ByOffset", title: "被充抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' }
        ]],
        url: "../Report/GetProveList", sortName: "ItemID", sortOrder: "asc",
        toolbar: buttonStr
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

clear = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "clear")) {
        var tempText = "加入补考费";
        if (clearStr === "No") {
            clearStr = "Yes";
            tempText = "加入补考费";
        }
        else {
            clearStr = "No";
            tempText = "清除补考费";
        }
        for (var i = 0; i < buttonStr.length; i++) {
            if (buttonStr[i].handler === "clear") {
                buttonStr[i].text = tempText;
                break;
            }
        }
        var queryData = rerurnQueryData();
        initTable(queryData);//将值传递给
        $("#grid").datagrid({
            toolbar: buttonStr
        });
    }
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../Report/ProveDownload",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.iserror != false) {
                    location.href = "../Temp/" + result.Message;
                }
            }
        });
    }
}

rerurnQueryData = function () {
    var data = {//得到用户输入的参数
        Clear: clearStr,
        MenuID: menuId,
        txtDeptID: "" + $("#DeptID").combotree("getValue"),
        txtFeeTimeS: "" + $("#txtFeeTimeS").combobox("getValue"),
        txtFeeTimeE: "" + $("#txtFeeTimeE").combobox("getValue")
    }
    return data;
}