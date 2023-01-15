import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { TabsModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { appRoutes } from './routes';
import { NavBarComponent } from './navBar/navBar.component';
import { FormsModule } from '@angular/forms';
import { AuthService } from './_services/auth.service';
import { PictureService } from './_services/picture.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { CanvasComponent } from './canvas/canvas.component';
import { FooterComponent } from './footer/footer.component';
//User
import { UserListComponent } from './userComponents/userList/userList.component';
import { UserCardComponent } from './userComponents/userCard/userCard.component';
import { UserSearchBarComponent } from './userComponents/userSearchBar/userSearchBar.component';
import { UserProfileComponent } from './userComponents/userProfile/userProfile.component';
import { UserComponent } from './userComponents/user/user.component';
import { UserSettingsComponent } from './userComponents/user-settings/user-settings.component';
//Comment
import { CommentProfileCardComponent } from './commentComponents/commentProfileCard/commentProfileCard.component';
import { CommentPictureCardComponent } from './commentComponents/commentPictureCard/commentPictureCard.component';
//Pictures
import { PictureMiniCardComponent } from './pictureComponents/pictureMiniCard/pictureMiniCard.component';
import { PictureCardComponent } from './pictureComponents/pictureCard/pictureCard.component';
import { PictureListComponent } from './pictureComponents/pictureList/pictureList.component';
import { PictureComponent } from './pictureComponents/picture/picture.component';
//Report
import { ReportComponent } from './reportComponents/report/report.component';
import { ReportListComponent } from './reportComponents/report-list/report-list.component';
import { ReportCardComponent } from './reportComponents/report-card/report-card.component';
import { AboutComponent } from './about/about.component';

export function tokenGetter(){
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [										
      AppComponent,
      UserComponent,
      NavBarComponent,
      HomeComponent,
      RegisterComponent,
      CanvasComponent,
      UserListComponent,
      UserCardComponent,
      UserProfileComponent,
      UserSearchBarComponent,
      UserSettingsComponent,
      CommentProfileCardComponent,
      FooterComponent,
      PictureCardComponent,
      PictureListComponent,
      PictureComponent,
      PictureMiniCardComponent,
      CommentPictureCardComponent,
      ReportComponent,
      AdminPanelComponent,
      ReportListComponent,
      ReportCardComponent,
      AboutComponent
   ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes, { scrollPositionRestoration: 'enabled' }),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains:['localhost:5050'],
        disallowedRoutes:['localhost:5050/api/auth']
      }
    })
  ],
  providers: [
    AuthService,
    PictureService
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule{}
