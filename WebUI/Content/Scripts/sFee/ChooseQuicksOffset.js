var feeDetailData = JSON.stringify(allDataGrid);
//收费项目列表
bindFeeDetailGrid = function () {
    $("#sfeeDetail").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        columns: [[
            { field: "sOrderID", hidden: true },
            { field: "DetailName", title: "收费类别", width: 115, sortable: true },
            { field: "ShouldMoney", title: "应缴金额", width: 70, sortable: true, align: 'right', halign: 'left' },
            { field: "UnPaid", title: "未充抵金额", width: 70, sortable: true, align: 'right', halign: 'left' }
        ]],
        data: JSON.parse(feeDetailData),
        onClickRow: function (rowIndex, rowData) {
            var index = bindGetTabsIndexMethod();
            var queryData = getQueueData();
            var gird = bindgetGridID(index);
            var urlstr = getDataUrl(index);
            $(gird).datagrid({
                url: urlstr,
                queryParams: queryData
            });
            //$("#sFeeOrder").edatagrid({ url: "../sFee/GetChoosesFeeList", queryParams: getQueueData() });
        },
        onLoadSuccess: function (data) {
            setTimeout(function () {
                for (var j = 0; j < totalData.length; j++) {
                    for (var i = 0; i < data.rows.length; i++) {
                        if (data.rows[i]["sOrderID"] === totalData[j]["sOrderID"]) {
                            var row = data.rows[i];
                            var index = $("#sfeeDetail").datagrid('getRowIndex', row);
                            var money = parseFloat(data.rows[i]["UnPaid"]) - parseFloat(totalData[j]["Money"]);
                            updateFeeDetailRows(index, money);
                        }
                    }

                }
            }, 1);
        }
    });
};

bindsFeesOffsetGrid = function () {
    $("#sFeesOffset").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行 
        pageSize: 20,
        data: totalData,
        columns: [[
            { field: "sOrderID", hidden: true },
            { field: "ID", hidden: true },
            { field: "Sort", hidden: true },
            { field: "Name", title: "学生姓名", width: 70, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "OffsetItem", title: "充抵到收费项", width: 100, sortable: true },
            { field: "Money", title: "充抵金额", width: 60, sortable: true, align: 'right', halign: 'left' },
            { field: "VoucherNum", title: "被充抵凭证号", width: 115, sortable: true },
            { field: "FeeContent", title: "被充抵收费类", width: 100, sortable: true },
            {
                field: "SortName", title: "被充抵类别", width: 70, sortable: true, formatter: function (value, row, index) {
                    if (row.Sort === "1") {
                        return "学费";
                    } else if (row.Sort === "2") {
                        return "杂费";
                    } else if (row.Sort === "3") {
                        return "证书费";
                    }
                }
            }
        ]],
        toolbar: [{
            id: "btnAddsOffset",
            iconCls: 'icon-add',
            text: "添加"
        }, '-', {
            id: "btnRemovesOffset",
            iconCls: 'icon-cancel',
            text: "移除"
        }]
    });
};
bindsFeeOrderGrid = function () {
    $("#sFeeOrder").edatagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: { MenuID: menuId },//异步查询的参数
        columns: [[
            { field: "ID", checkbox: true },
            { field: "Money", title: "充抵金额", width: 80, sortable: false, editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0 } } },
            { field: "CanMoney", title: "可充抵金额", sortable: true, align: 'right', halign: 'left' },
            { field: "StudName", title: "学生姓名", width: 60, sortable: true },
            { field: "FeeContent", title: "收费类别", width: 80, sortable: true },
            { field: "NumName", title: "缴费学年", width: 60, sortable: true },
            { field: "ProName", title: "专业", width: 120, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "DeptName", title: "收费单位", width: 200, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "Sort", hidden: true }
        ]],
        sortName: "FeeTime", sortOrder: "desc",
        onLoadSuccess: function (data) {
            var offsetData = $("#sFeesOffset").datagrid("getData");
            for (var j = 0; j < offsetData.rows.length; j++) {
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i]["ID"] === offsetData.rows[j]["ID"] && data.rows[i].Sort === offsetData.rows[j].Sort) {
                        var row = data.rows[i];
                        var index = $("#sFeeOrder").datagrid('getRowIndex', row);
                        var money = parseFloat(row["CanMoney"]) - parseFloat(offsetData.rows[j]["Money"]);
                        updateRows("#sFeeOrder", index, money, "0.00");
                    }
                }

            }
        }
    });
    //杂费
    $("#iFeeOffset").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        queryParams: { MenuID: menuId },
        pageSize: 10,
        columns: [[
            { field: "ID", checkbox: true },
            { field: "Money", title: "充抵金额", width: 80, sortable: false, editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0 } } },
            { field: "CanMoney", title: "可充抵金额", sortable: true, align: 'right', halign: 'left' },
            { field: "StudName", title: "学生姓名", width: 60, sortable: true },
            { field: "FeeContent", title: "收费类别", width: 80, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "DeptName", title: "收费单位", width: 200, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "Sort", hidden: true }
        ]],
        sortName: "ID", sortOrder: "desc",
        onLoadSuccess: function (data) {
            var offsetData = $("#sFeesOffset").datagrid("getData");
            for (var j = 0; j < offsetData.rows.length; j++) {
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i].ID === offsetData.rows[j].ID && data.rows[i].Sort === offsetData.rows[j].Sort) {
                        var row = data.rows[i];
                        var index = $("#iFeeOffset").datagrid('getRowIndex', row);
                        var money = parseFloat(row.CanMoney) - parseFloat(offsetData.rows[j].Money);
                        updateRows("#iFeeOffset", index, money, "0.00");
                    }
                }
            }
        }
    });
    //证书
    $("#FeeOffset").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        queryParams: { MenuID: menuId },
        pageSize: 10,
        columns: [[
            { field: "ID", checkbox: true },
            { field: "Money", title: "充抵金额", width: 80, sortable: false, editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0 } } },
            { field: "CanMoney", title: "可充抵金额", sortable: true, align: 'right', halign: 'left' },
            { field: "StudName", title: "学生姓名", width: 60, sortable: true },
            { field: "FeeContent", title: "收费类别", width: 80, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "DeptName", title: "收费单位", width: 200, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "Sort", hidden: true }
        ]],
        sortName: "ID", sortOrder: "desc",
        onLoadSuccess: function (data) {
            var offsetData = $("#sFeesOffset").datagrid("getData");
            for (var j = 0; j < offsetData.rows.length; j++) {
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i].ID === offsetData.rows[j].ID && data.rows[i].Sort === offsetData.rows[j].Sort) {
                        var row = data.rows[i];
                        var index = $("#FeeOffset").datagrid('getRowIndex', row);
                        var money = parseFloat(row.CanMoney) - parseFloat(offsetData.rows[j].Money);
                        updateRows("#FeeOffset", index, money, "0.00");
                    }
                }
            }
        }
    });
}


//获取查询参数
var getQueueData = function () {
    var queueData = {
        MenuID: menuId,
        studName: $("#txtChoStudName").textbox("getValue"),
        voucherNum: $("#txtChoVoucherNum").textbox("getValue"),
        idCard: $("#txtChoIdCard").textbox("getValue"),
        major: $("#txtMajor").textbox("getValue")
    }
    return queueData;
}
bindAddMethod = function () {
    $("#btnAddsOffset").click(function () {
        var rows = $("#sfeeDetail").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费类别")) {
            var index = bindGetTabsIndexMethod();
            var gird = bindgetGridID(index);
            $(gird).datagrid('acceptChanges');
            var row = $(gird).datagrid("getSelected");//获取选中行
            var index = $(gird).datagrid('getRowIndex', row);//获取选中行索引

            var itemdetial = $("#sfeeDetail").datagrid("getSelected");
            var orderIndex = $("#sfeeDetail").datagrid('getRowIndex', itemdetial);
            if (row != null && row != undefined) {
                if (itemdetial.length <= 0) {
                    Easy.centerShow("系统消息", "请选择收费项目名称！", 1000);
                    return false;
                }
                if (parseFloat(row.Money) > parseFloat(row.CanMoney)) {
                    Easy.centerShow("系统消息", "充抵金额不能大于可充抵金额！", 1000);
                    return false;
                }
                if (row.Money === undefined) {
                    Easy.centerShow("系统消息", "充抵金额不能为空！", 1000);
                    return false;
                }
                if (parseFloat(row.Money) === 0) {
                    Easy.centerShow("系统消息", "充抵金额不能为0！", 1000);
                    return false;
                }
                var money = itemdetial.UnPaid;
                if (parseFloat(row.Money) > parseFloat(money)) {
                    Easy.centerShow("系统消息", "充抵金额不能超过该收费项的金额！", 1000);
                    return false;
                }
                var data = $('#sFeesOffset').datagrid("getData");
                for (var i = 0; i < data.rows.length; i++) {
                    if (row.ID === data.rows[i]["ID"] && itemdetial.sOrderID === data.rows[i]["sOrderID"]) {
                        Easy.centerShow("系统消息", "同一收费项不能充抵两次给同一个收费项！", 1000);
                        return false;
                    }
                }
                $('#sFeesOffset').datagrid('appendRow',
                 {
                     ID: row.ID,
                     VoucherNum: row.VoucherNum,
                     Name: row.StudName,
                     IDCard: row.IDCard,
                     Money: row.Money,
                     sOrderID: itemdetial.sOrderID,
                     OffsetItem: itemdetial.DetailName,
                     FeeContent: row.FeeContent,
                     Sort: row.Sort
                 });
                var money = row.CanMoney - row.Money;
                var orderMoney = itemdetial.UnPaid - row.Money;
                updateRows(gird, index, money, "0.00");
                updateOrderRows(orderIndex, orderMoney);

            }
            else {
                Easy.centerShow("系统消息", "请选择收费信息！", 1000);
            }
        }
    });
}

updateRows = function (grid, index, money, offset) {
    $(grid).datagrid('updateRow', {
        index: index,
        row: {
            CanMoney: parseFloat(money).toFixed("2"),
            Money: parseFloat(offset).toFixed("2")
        }
    });
}
updateFeeDetailRows = function (index, money) {
    $("#sfeeDetail").datagrid('updateRow', {
        index: index,
        row: {
            UnPaid: parseFloat(money).toFixed("2")
        }
    });
}
updateOrderRows = function (index, money) {
    $("#sfeeDetail").datagrid('updateRow', {
        index: index,
        row: {
            UnPaid: parseFloat(money).toFixed("2")
        }
    });
}

bindRemoveMethod = function () {
    $("#btnRemovesOffset").click(function () {
        var row = $('#sFeesOffset').datagrid("getSelected");
        if (row != null) {
            var index = bindGetTabsIndexMethod();
            var gird = bindgetGridID(index);
            var ix0 = $('#sFeesOffset').datagrid('getRowIndex', row);
            var data = $(gird).datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                var newrow = data.rows[i];
                var index = $(gird).datagrid('getRowIndex', newrow);
                if (newrow.ID === row.ID) {
                    var money = parseFloat(newrow.CanMoney) + parseFloat(row.Money);
                    updateRows(gird, index, money, "0.00");
                }
            }
            var orderData = $("#sfeeDetail").datagrid("getData");
            for (var i = 0; i < orderData.rows.length; i++) {
                if (orderData.rows[i].sOrderID === row.sOrderID) {
                    var orderMoney = parseFloat(orderData.rows[i].UnPaid) + parseFloat(row.Money);
                    updateOrderRows(i, orderMoney);
                }
            }
            $('#sFeesOffset').datagrid('deleteRow', ix0);
        } else {
            Easy.centerShow("系统消息", "请选择收费信息！", 1000);
        }
    });
}
bindButtonMethod = function () {
    $("#btnSubmitForm").click(function () {
        var rows = $("#sfeeDetail").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费类别")) {
            var index = bindGetTabsIndexMethod();
            var queryData = getQueueData();
            var gird = bindgetGridID(index);
            var urlstr = getDataUrl(index);
            $(gird).datagrid({
                url: urlstr,
                queryParams: queryData
            });
        }

    });

    $("#btnResetForm").click(function () {
        var rows = $("#sfeeDetail").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费类别")) {
            $("#sOffsetForm").form("reset");//重置表单
            $("#sFeeOrder").datagrid({
                url: "../sFee/GetChoosesFeeList",
                queryParams: { MenuID: menuId }
            });
        }
    });
};
///获取当前选中标签的index
bindGetTabsIndexMethod = function () {
    var tab = $('#chooseFeeofsetTas').tabs('getSelected');
    var index = $('#chooseFeeofsetTas').tabs('getTabIndex', tab);
    return index;
};
bindloadList = function () {
    $('#chooseFeeofsetTas').tabs({
        selected: 0,
        onSelect: function (title, index) {
            var rows = $("#sfeeDetail").datagrid("getSelections");//选中的所有ID
            if (Easy.checkRow(rows, "收费类别")) {
                var queryData = getQueueData();
                var gird = bindgetGridID(index);
                var urlstr = getDataUrl(index);
                $(gird).datagrid({
                    url: urlstr,
                    queryParams: queryData
                });
            }
        }
    });
}
//获取grid表格ID
bindgetGridID = function (index) {
    var grid = "";
    if (index === 0)
        grid = "#sFeeOrder";
    else if (index === 1)
        grid = "#iFeeOffset";
    else if (index === 2)
        grid = "#FeeOffset";
    return grid;
};
//返回url
getDataUrl = function (index) {
    var url;
    if (index === 0)
        url = "../sFee/GetChoosesFeeList";
    else if (index === 1)
        url = "../OffsetChooser/GetiFeeList";
    else if (index === 2)
        url = "../OffsetChooser/GetFeeList";
    return url;
}
bindFeeDetailGrid();
bindsFeeOrderGrid();
bindsFeesOffsetGrid();
bindAddMethod();
bindRemoveMethod();
bindButtonMethod();
setTimeout(function () {
    bindloadList();
}, 1);