
import { NgModule, ErrorHandler } from '@angular/core';
import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';
import { MyApp } from './app.component'
import { aboutPage } from '../pages/about/about';
import { ContactPage} from '../pages/Contact/Contact';
import { HomePage } from '../pages/home/home';
import { TabsPage } from '../pages/Tabs/Tabs';
import { settingsPage } from '../pages/settings/settings';
import { accountPage } from '../pages/account/account';
import { ORDER_LISTPage } from '../pages/ORDER-LIST/ORDER-LIST';
import { worldnewsPage } from '../pages/worldnews/worldnews';

 
@NgModule({
    declarations: [
        MyApp,
        ContactPage,
        HomePage,
        TabsPage,
        ORDER_LISTPage,
        settingsPage,
        accountPage,
        aboutPage,
        worldnewsPage
    ],
    imports: [
        IonicModule.forRoot(MyApp)
    ],
    bootstrap: [IonicApp],
    entryComponents: [
        MyApp,
        ContactPage,
        HomePage,
        TabsPage,
        ORDER_LISTPage,
        settingsPage,
        accountPage,
        aboutPage,
        worldnewsPage
    ],
    providers: [{ provide: ErrorHandler, useClass: IonicErrorHandler }]
})
export class AppModule { }
