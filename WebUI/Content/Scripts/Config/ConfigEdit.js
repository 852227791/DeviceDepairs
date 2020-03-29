
bindSelectInfo = function () {
    var rows = $("#grid").datagrid("getSelections"); 
    if (rows.length > 0) {

        $("#ConfigID").textbox('setValue', rows[0].ConfigID);
        $("#PrintNum").numberspinner('setValue', rows[0].PrintNum);
    }
}
setTimeout("bindSelectInfo()", 1);