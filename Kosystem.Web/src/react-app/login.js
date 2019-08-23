var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
import React, { Component } from 'react';
var Login = /** @class */ (function (_super) {
    __extends(Login, _super);
    function Login() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Login.prototype.render = function () {
        return (React.createElement("form", null,
            React.createElement("fieldset", null,
                React.createElement("legend", null, "G\u00E5 med i r\u00E4tt k\u00F6"),
                React.createElement("label", null, "Namn"),
                React.createElement("input", { type: "text", name: "name", placeholder: "F\u00F6rnamn Efternamn" }))));
    };
    return Login;
}(Component));
export default Login;
//# sourceMappingURL=login.js.map