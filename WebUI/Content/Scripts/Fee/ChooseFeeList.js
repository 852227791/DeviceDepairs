var index = undefined;
var fee = null;

initChoosefeeTable1 = function (queryData) {
    $("#editItemDetailList").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        columns: [[
            { field: "ItemDetailID", hidden: true },
            { field: "OffsetItem", title: "收费项", width: 115, sortable: true }
        ]], data: JSON.parse($("#editItemDetailJson").val()),
        onClickRow: function (rowIndex, rowData) {
            fee.datagrid({
                url: "../Fee/GetChooseFeeList",
                queryParams: rerurnQueryData()
            });
        }
    });
    $("#choosefeegrid2").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行 
        pageSize: 20,
        columns: [[
            { field: "ItemDetailID", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 115, sortable: true },
            { field: "Name", title: "学生姓名", width: 80, sortable: true },
            { field: "Dept", title: "收费单位", width: 80, sortable: true },
            { field: "ItemName", title: "证书名称", width: 60, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 100, sortable: true },
            { field: "Offset", title: "充抵金额", width: 100, sortable: true, align: 'right' },
            { field: "OffsetItem", title: "充抵到收费项", width: 100, sortable: true }
        ]]
    });

    if (tempData != null) {
        $("#choosefeegrid2").datagrid("loadData", tempData);
    }
    fee = $("#choosefeegrid1").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "FeeDetailID", hidden: true },
            { field: "Offset", title: "充抵金额", width: 70, sortable: false, editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0 } } },
            { field: "CanMoney", title: "可充抵金额", width: 70, sortable: true, align: 'right' },
            { field: "VoucherNum", title: "凭证号", width: 115, sortable: true },
            { field: "Name", title: "学生姓名", width: 80, sortable: true },
            { field: "Dept", title: "收费单位", width: 80, sortable: true },
            { field: "ItemName", title: "证书名称", width: 60, sortable: true },
            { field: "DetailName", title: "收费项", width: 60, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 100, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true }
        ]],
        sortName: "FeeTime", sortOrder: "desc",
        onLoadSuccess: function (data) {
            var choosedData = $('#choosefeegrid2').datagrid("getData");
            if (choosedData.rows.length > 0) {
                for (var i = 0; i < data.rows.length; i++) {
                    for (var j = 0; j < choosedData.rows.length; j++) {
                        if (data.rows[i]["FeeDetailID"] === choosedData.rows[j]["FeeDetailID"]) {
                            var row = data.rows[i];
                            var index = fee.datagrid('getRowIndex', row);
                            var money = parseFloat(data.rows[i]["CanMoney"]) - (choosedData.rows[j]["Offset"]);
                            updateRows(index, money, "0.00");
                        }
                    }
                }
            }
        }
    });
}


//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearchChooseFee").click(function () {
        var rows = $("#editItemDetailList").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费项")) {
            var queryData = rerurnQueryData();
            fee.datagrid({
                url: "../Fee/GetChooseFeeList",
                queryParams: queryData
            });
        }

    });

    $("#btnResetChooseFee").click(function () {
        var rows = $("#editItemDetailList").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费项")) {
            $("#fSearchChooseFee").form("reset");//重置表单
            fee.datagrid({
                url: "../Fee/GetChooseFeeList"
            });
        }
        ///  initChoosefeeTable1({ DeptID: DeptID, MenuID: menuId });
    });
}
rerurnQueryData = function () {

    var queryData = {
        MenuID: menuId,
        DeptID: DeptID,
        voucherNum: $("#txtVoucherChooseFee").val(),
        studentName: $("#txtStudentNameChooseFee").val()
    }
    return queryData;
}
//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#selDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 200
    });
}


Add = function () {
    $("#btnAdd").click(function () {
        fee.datagrid('acceptChanges');
        var row = fee.datagrid("getSelected");//获取选中行
        var index = fee.datagrid('getRowIndex', row);//获取选中行索引
        var itemdetial = $("#editItemDetailList").datagrid("getSelections");
        if (row != null && row != undefined) {
            if (itemdetial.length <= 0) {
                Easy.centerShow("系统消息", "请选择收费项目名称！", 1000);
                return false;
            }
            if (parseFloat(row.Offset) > parseFloat(row.CanMoney)) {
                Easy.centerShow("系统消息", "充抵金额不能大于可充抵金额！", 1000);
                return false;
            }
            if (row.Offset === undefined) {
                Easy.centerShow("系统消息", "充抵金额不能为空！", 1000);
                return false;
            }
            if (parseFloat(row.Offset) === 0) {
                Easy.centerShow("系统消息", "充抵金额不能为0！", 1000);
                return false;
            }
            var money = itemdetial[0]["OffsetItem"].split(' ');
            if (parseFloat(row.Offset) > parseFloat(money[1])) {
                Easy.centerShow("系统消息", "充抵金额不能超过该收费项的金额！", 1000);
                return false;
            }
            var data = $('#choosefeegrid2').datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                if (row.FeeDetailID === data.rows[i]["FeeDetailID"] && itemdetial[0].ItemDetailID === data.rows[i].ItemDetailID) {
                    Easy.centerShow("系统消息", "同一收费项不能充抵两次给同一个收费项！", 1000);
                    return false;
                }
            }
            $('#choosefeegrid2').datagrid('appendRow',
             {
                 FeeDetailID: row.FeeDetailID,
                 VoucherNum: row.VoucherNum,
                 Name: row.Name,
                 Dept: row.Dept,
                 NoteNum: row.NoteNum,
                 ItemName: row.ItemName,
                 FeeTime: row.FeeTime,
                 CanMoney: row.CanMoney,
                 Offset: row.Offset,
                 ItemDetailID: itemdetial[0].ItemDetailID,
                 OffsetItem: itemdetial[0]["OffsetItem"]

             });
            var money = row.CanMoney - row.Offset;
            updateRows(index, money, "0.00");

        }
        else {
            Easy.centerShow("系统消息", "请选择收费信息！", 1000);
        }

    });
}

updateRows = function (index, money, offset) {
    fee.datagrid('updateRow', {
        index: index,
        row: {
            CanMoney: parseFloat(money).toFixed("2"),
            Offset: parseFloat(offset).toFixed("2")
        }
    });
}

remove = function () {
    $("#btnRemove").click(function () {
        var row = $('#choosefeegrid2').datagrid("getSelected");
        if (row != null) {
            var ix0 = $('#choosefeegrid2').datagrid('getRowIndex', row);
            var data = fee.datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                var newrow = data.rows[i];
                var index = fee.datagrid('getRowIndex', newrow);
                if (newrow.FeeDetailID === row.FeeDetailID) {
                    var money = parseFloat(newrow.CanMoney) + parseFloat(row.Offset);
                    updateRows(index, money, "0.00");
                }
            }
            $('#choosefeegrid2').datagrid('deleteRow', ix0);
        } else {
            Easy.centerShow("系统消息", "请选择收费信息！", 1000);
        }


    });
}
initChoosefeeTable1({ DeptID: DeptID, MenuID: menuId });
bindSearchClickEvent();
remove();
Add();
bindOffsetMoney = function () {
    if (FeeID != "0") {
        $("#choosefeegrid2").edatagrid({ data: JSON.parse($("#editOffsetID").textbox("getValue")) });
    }
    else {
        $("#choosefeegrid2").edatagrid({ data: [] });
    }
}
