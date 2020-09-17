
let ABCAutoTest = {
  
  //启动自动化测试插件
  ExecuteScript() {
    cordova.exec(result => {
      console.log("调用插件成功:", result);
    }, error => {
      console.log("调用插件失败：", error);
    }, 'ABCSocketServer', 'Initialize');
  },
  //开始录制
  StartRecordTrade() {
    cordova.exec(result => {
      console.log("调用插件成功:", result);
    }, error => {
      console.log("调用插件失败：", error);
    }, 'RecordTradeCase', 'SetStartFlag');
  },
  //结束录制
  EndRecordTrade() {
    cordova.exec(result => {
      console.log("调用插件成功:", result);
    }, error => {
      console.log("调用插件失败：", error);
    }, 'RecordTradeCase', 'SetEndFlag');
  },
  //获取当前的录制状态
  getBoolRecord() {
    cordova.exec(result => {
      console.log("调用插件成功:", result);
      window.flag = result;
    }, error => {
      console.log("调用插件失败：", error);
    }, 'RecordTradeCase', 'GetFlag');
  },
  //获取录制的组件的信息
  getRecContext(recContext) {
    cordova.exec(result => {
      console.log("调用插件成功:", result);
    }, error => {
      console.log("调用插件失败：", error);
    }, 'RecordTradeCase', 'GetRecContext', [recContext]);
  },
  //调用插件获取录制信息
  RecordTrade() {
    cordova.exec(result => {
      console.log("调用插件成功" + result);
      alert("调用插件成功");
    }, error => {
      console.log("调用插件失败" + error);
      alert("调用插件失败");
    }, 'RecordTradeCase', 'GetTradeRecord');
  },
  //获取执行交易的客户端信息
  GetClientInfo() {
    cordova.exec(result => {
      console.log("调用插件成功:1", result);
    }, error => {
      console.log("调用插件失败：", error);
    }, 'TradeContextProvider', 'GetClientInfo', []);
  },
   //获取执行交易码
   getTradeCode() {
    cordova.exec(result => {
      console.log("调用插件成功:2", result);
    }, error => {
      console.log("调用插件失败：", error);
    }, 'TradeContextProvider', 'GetTradeCode', []);
  },
  //获取执行交易的设备信息
  getSequenceNumber() {
   cordova.exec(result => {
     console.log("调用插件成功3:", result);
   }, error => {
     console.log("调用插件失败：", error);
   }, 'TradeContextProvider', 'GetSequenceNumber', []);
 }
};
export {
  ABCAutoTest
};
