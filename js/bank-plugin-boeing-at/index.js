import Vue from 'vue';
import DiologNews from '';
import DialogConmment from '';

// 引入日志记录类
import { NativeLogger } from '@agree-sdk/awp-plugin-logger';

// 新建Logger对象，其中XXLoggerName为对应log4net中配置的Logger名称
let testPluginLogger = new NativeLogger('testPluginLogger');

// 记录日志
//XXXLogger.debug('message', objectXX);

//自动化测试
import {
    ABCAutoTest
} from './at-plugins/ABCTest';
import ATBase from './at-plugins/at-base.js';
import ATVueControlRec from './at-plugins/at-vue-control-rec.js';
import ATNativeControlRec from './at-plugins/at-native-control-rec.js';

import BuryingPoint from './at-plugins/burying-point.js';
import BuryingVueControlRec from './at-plugins/VueBurying.js';
import BuryingNativeControlRec from './at-plugins/NativeBurying.js';


export default {
    install(Vue, options) {
        Vue.use(ATBase);
        Vue.use(ATVueControlRec, {
            type: '$emit'
        });
        Vue.mixin(ATNativeControlRec);

        Vue.use(BuryingPoint);
        Vue.mixin(BuryingPoint);
        Vue.use(BuryingVueControlRec, {
            type: '$emit'
        });
       Vue.mixin(BuryingNativeControlRec);

        console.log('插件加载');
        var message = '测试通过';
        testPluginLogger.debug('message',message);

        //启动自动化测试插件
        //ABCAutoTest.ExecuteScript();
        ABCAutoTest.getBoolRecord();
        ABCAutoTest.GetClientInfo();
        ABCAutoTest.getTradeCode();
        ABCAutoTest.getSequenceNumber();
        
    }
}