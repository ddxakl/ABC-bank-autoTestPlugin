
/**
 * Created by zhaoyuchen on 2019/01/14.
 * 录制时,获取原生组件信息
 */
import {ABCAutoTest} from "./ABCTest";
export default {
  mounted() {
    let _this = this;
    this._vnode.children && this._vnode.children.forEach(element => {
      let tag = element.tag;
      if (tag === undefined) return;
      if (tag === "input") {
        getInputTagData(element, _this);
      } else if (tag === "textarea") {
        getTextareaTagData(element, _this);
      } else if (tag === "button") {
        getButtonTagData(element, _this);
      } else if (tag === "select") {
        getSelectTagData(element, _this);
      } else if (tag === "img") {
        getImgTagDta(element, _this);
      } else if (tag === "div") {
        getDivTagData(element, _this);
        element.children && getChildrenTag(element.children, _this);
      }
    });
  }
};

function getChildrenTag(children, _this) {
  for (let i = 0; i < children.length; i++) {
    if (children[i].tag === "input") {
      getInputTagData(children[i], _this);
    } else if (children[i].tag === "textarea") {
      getTextareaTagData(children[i], _this);
    } else if (children[i].tag === "button") {
      getButtonTagData(children[i], _this);
    } else if (children[i].tag === "select") {
      getSelectTagData(children[i], _this);
    } else if (children[i].tag === "img") {
      getImgTagDta(children[i], _this);
    } else if (children[i].tag === "div") {
      getDivTagData(children[i], _this);
      children[i].children && getChildrenTag(children[i].children, _this);
    }
  }
}

function getInputTagData(VNode, _this) {
  if (VNode.data && VNode.data.on) {
    if (VNode.data.on.blur) {
      let myFn = VNode.data.on.blur.fns;
      VNode.data.on.blur.fns = function ($event) {
        getControlInfo(VNode, _this);
        myFn();
      };
    } else {
      VNode.elm.addEventListener("blur", function() {
        
        getControlInfo(VNode, _this);
      });
    }
  }
}

function getTextareaTagData(VNode, _this) {
  getInputTagData(VNode, _this);
}

function getButtonTagData(VNode, _this) {
  if (VNode.data && VNode.data.on && VNode.data.on.click) {
    let myFn = VNode.data.on.click.fns;
    VNode.data.on.click.fns = function ($event) {
      getControlInfo(VNode, _this);
      myFn();
    };
  }
}

function getSelectTagData(VNode, _this) {
  if (VNode.data && VNode.data.on && VNode.data.on.change) {
    let myFn = VNode.data.on.change.fns;
    VNode.data.on.change.fns = function ($event) {
      getControlInfo(VNode, _this);
      myFn($event);
    };
  }
}

function getImgTagDta(VNode, _this) {
  if (VNode.data && VNode.data.on && VNode.data.on.click) {
    let myFn = VNode.data.on.click.fns;
    VNode.data.on.click.fns = function ($event) {
      getControlInfo(VNode, _this);
      myFn();
    };
  }
}

function getDivTagData(VNode, _this) {
  if (VNode.data && VNode.data.on && VNode.data.on.click) {
    VNode.elm.addEventListener("click", function() {
      getControlInfo(VNode, _this);
    });
  }
}

function getControlInfo(VNode, _this) {
  console.log("native:", VNode);
  // let recContext = window.recContext;
  let controlInfo = {};
  controlInfo['tradeCode'] = VNode.context.constructor.extendOptions.name;
  controlInfo['pageCode'] = VNode.context.constructor.extendOptions.name;
  controlInfo['id'] = VNode.data.ref+"#";
  controlInfo['controlModel'] = VNode.data.directives === undefined ? "" : VNode.data.directives[0].expression;
  controlInfo['controlType'] = "";
  controlInfo['value'] = "";
  controlInfo['valueAdd'] = "";

  if (VNode.tag === "input" ) {
    controlInfo['controlType'] = 'Text';
    controlInfo['value'] = VNode.elm.value;
  } else if (VNode.tag === "textarea") {
    controlInfo['controlType'] = 'MultiLineText';
    controlInfo['value'] = VNode.elm.value;
  } else if (VNode.tag === "button"||VNode.tag === "div") {
    controlInfo['controlType'] = 'NButton';
    if (VNode.data.key != undefined) {
      controlInfo['value'] = VNode.data.key;
      controlInfo['valueAdd'] = 'FB';
    }
  } else if (VNode.tag === "select") {
    controlInfo['controlType'] = 'Select';
    controlInfo['value'] = VNode.elm.value;
  } else if (VNode.tag === "img" ) {
    controlInfo['controlType'] = 'Image';
  }
  // else if(VNode.tag === "div"){
  //   controlInfo['controlType'] = 'button';
  // }
  //点击一次录制一次记录一次
  window.controlMap = controlInfo;
  
  //ABCAutoTest.getRecContext(JSON.stringify(window.controlMap));
  var name = controlInfo['pagecode'];
  for (var i = window.tradeInfo['tradePageList'].length - 1; i >= 0; i--) {
    console.log(name+'------------------------'+window.tradeInfo['tradePageList'][i]['pageName']);
    if (name === window.tradeInfo['tradePageList'][i]['pageName']) {
      //pageControlInfo['controlNum'] = window.tradeInfo['tradePageList'][i]['pageControlList'].length + 1;
      window.tradeInfo['tradePageList'][i]['pageControlList'].push(controlInfo);
      console.log("pageControlList的长度----"+ window.tradeInfo['tradePageList'][i]['pageControlList'].length);
      break;
    }
  }
}
