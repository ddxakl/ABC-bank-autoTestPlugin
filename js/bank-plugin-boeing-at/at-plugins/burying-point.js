window.tradeInfo = {}
export default {
    install(Vue, PluginOption = {}) {

        var netAdress = {};
        var deviceInfo = {};
        var userData = {};
        var applicationInfo = {};
        var tradePageList = [];
        var transPage = {};
        var transItem = {};
        window.bflag=true;

        deviceInfo = {};//每次执行之前清空数据，重新写入
        //获取客户端安装的的设备信息和用户信息
        cordova.exec(result => window.getDeviceInfo(result), error => console.error(error), 'TradeContextProvider', 'GetClientInfo', []);
        cordova.exec(result => window.getTradeCode(result), error => console.error(error), 'TradeContextProvider', 'GetTradeCode', []);
        //获取设备相关信息，用户信息
        window.getDeviceInfo = function (result) {
            deviceInfo['AccountDateOfABIS'] = result.AccountDateOfABIS;//会计日期
            deviceInfo['Channel'] = result.Channel;//渠道号
            deviceInfo['CodBrchOu'] = result.CodBrchOu;//组织单元代码
            deviceInfo['FinancialEntityCode'] = result.FinancialEntityCode;//0103
            deviceInfo['IPV4Address'] = result.IPV4Address;//IPV4地址
            deviceInfo['IPV6Address'] = result.IPV6Address;//IPV6地址
            deviceInfo['MACAddress'] = result.MACAddress;//物理设备号
            deviceInfo['Passage'] = result.Passage;//通道号
            netAdress['ProvinceCode'] = result.ProvinceCode;//省市代码地址
            deviceInfo['TerminalId'] = result.TerminalId;//终端号（设备号）
            userData['UserId'] = result.UserId;//用户ID
            userData['UserName'] = result.UserName;//柜员名称
        };
        //获取交易信息
        window.getTradeCode = function (result) {
            window.tradeInfo['tradeCode'] = result;
        };
        //获取用户操作时间信息
        var newDate = new Date();
        //window.userInfo['uid'] =deviceInfo['deviceNum'] + newDate.getTime();
        userData['handleStartDate'] = format(newDate);
        userData['loginTime'] = format(newDate);
        userData['handleQuitTime'] = "";//通过closeAll方法获取

        //获取交易信息
        window.tradeInfo['tradeNum'] = '';//交易码（可能没有此属性）
        window.tradeInfo['trademe'] = '';//交易名称
        window.tradeInfo['tradeCodNae'] = '';//交易代码
        //交易起止时间
        window.tradeInfo['tradeStartTime'] = format(newDate);
        window.tradeInfo['tradeEndTime'] = "";
        window.tradeInfo['tradeDuration'] = "";
        window.tradeInfo['tradeStatus'] = "";//交易状态
        window.tradeInfo['tradeFlow'] = "";//交易流
        window.tradeInfo['tradePageList'] = [];
        console.log("获取到的信息" + window.tradeInfo);


        transPage['tradeItem'] = transItem;//点击的组件的信息

        //组件信息
        transItem['itemName'] = '';
        transItem['itemStartTime'] = '';
        transItem['itemEndTime'] = '';
        transItem['itemTimeSpan'] = '';
        transItem['value'] = '';

        //数据埋点信息汇总
        window.buryingPointData = {};
        window.buryingPointData['netAdress'] = netAdress;
        window.buryingPointData['deviceInfo'] = deviceInfo;
        window.buryingPointData['userData'] = userData;
        window.buryingPointData['applicationInfo'] = applicationInfo;

       // console.log("数据埋点" + window.buryingPointData);


    },

    //获取vue页面的创建和销毁时间
    mounted() {
        if (this.$options != undefined && this.$options._componentTag === undefined) {
            if (this.constructor != undefined && this.constructor.extendOptions != undefined && this.constructor.extendOptions.name != undefined) {
                var name = this.constructor.extendOptions.name;
                window.window.transPage = {};
                window.transPage['pageNum'] = "";
                window.transPage['pageName'] = name;
                window.transPage['pageStartTime'] = format(new Date());
                window.transPage['pageEndTime'] = "";
                window.transPage['pageDuration'] = "";
                window.transPage['pageControlList'] = [];
                // var tradePageList = new Array();
                // tradePageList.push(window.transPage);
                window.tradeInfo['tradePageList'].push(window.transPage);//获取到所有的页面的
            }
        }
    },
    destroyed() {
        if (this.$options != undefined && this.$options._componentTag === undefined) {
            if (this != undefined && this.constructor != undefined && this.constructor.extendOptions != undefined && this.constructor.extendOptions.name != undefined) {
                var name = this.constructor.extendOptions.name;
                for (var i = tradeInfo['tradePageList'].length - 1; i >= 0; i--) {
                    if (name === window.tradeInfo['tradePageList'][i]['pageName']) {
                        var pageEndTime = new Date();
                        var pageStartTime = window.tradeInfo['tradePageList'][i]['pageStartTime'];
                        if (window.tradeInfo['tradePageList'][i]['pageEndTime'] === "") {
                            window.tradeInfo['tradePageList'][i]['pageStartTime'] = pageStartTime;
                            console.log("操作页面开始的时间"+window.tradeInfo['tradePageList'][i]['pageStartTime'])
                            window.tradeInfo['tradePageList'][i]['pageEndTime'] = format(pageEndTime);
                            console.log("操作页面结束的时间"+ window.tradeInfo['tradePageList'][i].pageEndTime);
                            window.tradeInfo['tradePageList'][i]['pageDuration'] = (format(pageEndTime) - pageStartTime) ;
                            break;
                        }
                    }
                }
            }
        }
    },

    SaveBuryingPointData() {

        var newDate = new Date();

        //console.log('打开的页面数：------------'+window.tradeInfo['tradePageList'].length );
        //筛选出没有进行组件操作的页面
        for (var i = window.tradeInfo['tradePageList'].length - 1; i >= 0; i--) {
            if(window.tradeInfo['tradePageList'][i].pageEndTime===""){
                window.tradeInfo['tradePageList'][i].pageEndTime=format(new Date());
                window.tradeInfo['tradePageList'][i].pageDuration=window.tradeInfo['tradePageList'][i].pageEndTime-window.tradeInfo['tradePageList'][i].pageStartTime;
            }
            if (window.tradeInfo['tradePageList'][i]['pageControlList'].length === 0) {
                window.tradeInfo['tradePageList'].splice(i, 1);
            }
            
            
        }

        console.log('window.tradeInfo-----------' + JSON.stringify(window.tradeInfo));
        window.buryingPointData['tradeInfo'] = window.tradeInfo;
        //交易结束时间和持续时间
        window.buryingPointData['tradeInfo']['tradeEndTime'] = format(newDate);
        window.buryingPointData['tradeInfo']['tradeDuration'] = window.buryingPointData['tradeInfo']['tradeEndTime'] - window.buryingPointData['tradeInfo']['tradeStartTime'];
        //用户操作结束时间
        window.buryingPointData['userData']['handleQuitTime'] = format(newDate);

        var data = window.buryingPointData;

       // console.log("数据埋点数据" + JSON.stringify(window.buryingPointData));
        cordova.exec(result => { console.log('埋点数据存储到本地') }, error => { console.log('埋点数据保存出错') }, 'SaveBuryPoint', 'SaveData', [data]);

    }

}

function format(newDate) {
    var fullYear = newDate.getFullYear();
    var month = (newDate.getMonth() + 1) < 10 ? "0" + (newDate.getMonth() + 1) : (newDate.getMonth() + 1);
    var date = newDate.getDate() < 10 ? "0" + newDate.getDate() : newDate.getDate();
    var hours = newDate.getHours() < 10 ? "0" + newDate.getHours() : newDate.getHours();
    var minutes = newDate.getMinutes() < 10 ? "0" + newDate.getMinutes() : newDate.getMinutes();
    var seconds = newDate.getSeconds() < 10 ? "0" + newDate.getSeconds() : newDate.getSeconds();
    return  fullYear+month+date+ hours+ minutes + seconds;
}

function StartGetBuryingPointData() {



}
// function SaveBuryingPointData() {
//     var newDate = new date();

//     //筛选出没有进行组件操作的页面
//     for (var i = window.tradeInfo['tradePageList'].length - 1; i >= 0; i--) {
//         if (window.tradeInfo['tradePageList'][i]['pageControlList'].length === 0) {
//             window.tradeInfo['tradePageList'].splice(i, 1);
//         }
//     }

//     window.buryingPointData['tradeInfo'] = window.tradeInfo;
//     //交易结束时间和持续时间
//     window.buryingPointData['tradeInfo']['tradeEndTime'] = format(newDate.getHours(), newDate.getMinutes(), newDate.getSeconds());
//     window.buryingPointData['tradeInfo']['tradeDuration'] = window.buryingPointData['tradeInfo']['tradeEndTime'] - window.buryingPointData['tradeInfo']['tradeStartTime'];
//     //用户操作结束时间
//     window.buryingPointData['userData']['handleQuitTime'] = format(newDate.getHours(), newDate.getMinutes(), newDate.getSeconds());
//     console.log("数据埋点数据" + window.buryingPointData);

//     cordova.exec(result => { console.log('埋点数据存储到本地') }, error => { console.log('埋点数据保存出错') }, 'SaveBuryPoint', 'SaveData', window.buryingPointData);

// }