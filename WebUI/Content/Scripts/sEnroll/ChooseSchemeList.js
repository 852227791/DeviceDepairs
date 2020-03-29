var scheme = null;

//查询条件数据
rerurnChooseSchemeQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        Major: sEnrollMajor,
        Year: sEnrollYear,
        Month: sEnrollMonth,
        txtSchemeName: $("#txtSchemeName").textbox("getValue"),
        Level: enrollLevel
    }
    return queryData;
}

//绑定表单1数据
initChooseSchemeGrid1 = function (queryData) {
    scheme = $("#chooseschemegrid1").datagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "ItemID", hidden: true },
            { field: "Year", title: "年份", width: 80, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "Name", title: "方案名称", width: 300, sortable: true }
        ]],
        url: "../sEnroll/GetChooseSchemeList", sortName: "Queue", sortOrder: "asc"
    });
}

//绑定表单2数据
initChooseSchemeGrid2 = function () {
    $("#chooseschemegrid2").datagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        columns: [[
            { field: "ItemID", hidden: true },
            { field: "Year", title: "年份", width: 80, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "Name", title: "方案名称", width: 300, sortable: true }
        ]]
    });
    var tempData = sEnrollSchemeTempData;
    if (tempData != null && tempData != "") {
        $("#chooseschemegrid2").datagrid("loadData", JSON.parse(tempData));
    }
}

//绑定搜索按钮的点击事件
bindChooseSearchClickEvent = function () {
    $("#btnSearchChooseScheme").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnChooseSchemeQueryData();
        initChooseSchemeGrid1(queryData);//将值传递给
    });
    $("#btnResetChooseScheme").click(function () {
        $("#fSearchChooseScheme").form("reset");//重置表单
        initChooseSchemeGrid1({ MenuID: menuId, Major: sEnrollMajor, Year: sEnrollYear, Month: sEnrollMonth, Level: enrollLevel });
    });
}

//添加事件
Add = function () {
    $("#btnAdd").click(function () {
        var row = scheme.datagrid("getSelected");//获取选中行
        var choData = $('#chooseschemegrid2').datagrid("getData");
        if (row != null) {
            var data = $('#chooseschemegrid2').datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                if (row.ItemID == data.rows[i]["ItemID"]) {
                    Easy.centerShow("系统消息", "同一缴费方案不能重复添加", 1000);
                    return false;
                }
            }

            var enrollLevel = "0";
            if (chooseSchame==="1") {
                enrollLevel = $("#enrollLevel").combobox("getValue");
            }
            else if (chooseSchame === "2") {
                enrollLevel = $("#MajorEnrollLevel").combobox("getValue");
            }
            var flag = true;
            if (enrollLevel == "1" || enrollLevel == "2" || enrollLevel == "3") {
                if (choData.rows.length+1 > 1) {
                    flag= false;
                }
            }
            else if (enrollLevel == "4" || enrollLevel == "5") {
                if (choData.rows.length + 1 > 2) {
                    flag = false;
                }
            }
            else if (enrollLevel == "6") {
                if (choData.rows.length + 1 > 3) {
                    flag = false;
                }
            }
            if (!flag) {
                Easy.centerShow("系统消息", "方案数量不匹配", 2000);
                return false;
            }
            $('#chooseschemegrid2').datagrid('appendRow', {
                ItemID: row.ItemID,
                Year: row.Year,
                Month: row.Month,
                Name: row.Name
            });
        }
        else {
            Easy.centerShow("系统消息", "请选择缴费方案", 1000);
        }
    });
}

//移除事件
Remove = function () {
    $("#btnRemove").click(function () {
        var row = $('#chooseschemegrid2').datagrid("getSelected");
        if (row != null) {
            var index2 = $('#chooseschemegrid2').datagrid('getRowIndex', row);
            $('#chooseschemegrid2').datagrid('deleteRow', index2);
        } else {
            Easy.centerShow("系统消息", "请选择缴费方案", 1000);
        }
    });
}

//加载表单2数据
initChooseSchemeGrid2();


//加载表单1数据
initChooseSchemeGrid1({ MenuID: menuId, Major: sEnrollMajor, Year: sEnrollYear, Month: sEnrollMonth, Level: enrollLevel });

//加载搜索按钮的点击事件
bindChooseSearchClickEvent();

//添加
Add();

//移除
Remove();
