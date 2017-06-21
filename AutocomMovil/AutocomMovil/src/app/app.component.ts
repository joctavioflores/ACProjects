import { Component, ViewChild } from '@angular/core';
import { Nav, Platform } from 'ionic-angular';
import { StatusBar, Splashscreen } from 'ionic-native';

import { TabsPage } from '../pages/Tabs/Tabs';
import { settingsPage } from '../pages/settings/settings';
import { accountPage } from '../pages/account/account';

import firebase from 'firebase';

@Component({
    templateUrl: 'app.html'
})
export class MyApp {
    @ViewChild(Nav) nav: Nav;

    rootPage = TabsPage;

    pages: Array<{ title: string, component: any }>;

    constructor(public platform: Platform) {

        const firebaseConfig = {
            apiKey: "AIzaSyBNBzE9DjZ1wncQdO8yduS-FYcGdtfziPk",
            authDomain: "autocommovildb.firebaseapp.com",
            databaseURL: "https://autocommovildb.firebaseio.com",
            projectId: "autocommovildb",
            storageBucket: "autocommovildb.appspot.com",
            messagingSenderId: "988151846498"
        };

        firebase.initializeApp(firebaseConfig);

        this.initializeApp();

        // used for an example of ngFor and navigation
        this.pages = [
            { title: 'Inicio', component: TabsPage },
            { title: 'Inicio', component: TabsPage },           
            { title: 'Perfil', component: accountPage },
            { title: 'Ajustes', component: settingsPage }
        ];

    }
    openPage(page) {
        // Reset the content nav to have just this page
        // we wouldn't want the back button to show in this scenario
        this.nav.setRoot(page.component);
    }

    initializeApp() {
        this.platform.ready().then(() => {
            // Okay, so the platform is ready and our plugins are available.
            // Here you can do any higher level native things you might need.
            StatusBar.styleDefault();
            Splashscreen.hide();
        });
    }
}
