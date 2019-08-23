import React, { Component } from 'react';

export default class Login extends Component {
    render() {
        return (
            <form>
                <fieldset>
                    <legend>
                        Gå med i rätt kö
                    </legend>

                    <label>Namn</label>
                    <input type="text" name="name" placeholder="Förnamn Efternamn" />
                </fieldset>
            </form>
        );
    }
}