import { Component } from '@angular/core';

import { HomePage } from '../home/home';
import { ORDER_LISTPage } from '../ORDER-LIST/ORDER-LIST';
import { ContactPage } from '../Contact/Contact';

@Component({
    templateUrl: 'tabs.html'
})
export class TabsPage {
    // this tells the tabs component which Pages
    // should be each tab's root Page
    tab1Root: any = HomePage;
    tab2Root: any = ORDER_LISTPage;
    tab3Root: any = ContactPage;

    constructor() {

    }
}