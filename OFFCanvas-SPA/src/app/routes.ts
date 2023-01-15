import { Routes } from "@angular/router";
import { AboutComponent } from "./about/about.component";
import { AdminPanelComponent } from "./admin-panel/admin-panel.component";
import { CanvasComponent } from "./canvas/canvas.component";
import { HomeComponent } from "./home/home.component";
//import { PictureComponent } from "./pictureComponents/picture/picture.component";
import { PictureComponent} from "./pictureComponents/picture/picture.component";
import { PictureListComponent } from "./pictureComponents/pictureList/pictureList.component";
import { RegisterComponent } from "./register/register.component";
import { ReportComponent } from "./reportComponents/report/report.component";
import { UserSettingsComponent } from "./userComponents/user-settings/user-settings.component";
import { UserComponent } from "./userComponents/user/user.component";
import { UserListComponent } from "./userComponents/userList/userList.component";
import { UserProfileComponent } from "./userComponents/userProfile/userProfile.component";
import { AuthGuard } from "./_guards/auth.guard";
import { ModGuard } from "./_guards/mod.guard";

export const appRoutes: Routes = [
    {path: 'home', component: HomeComponent},
    {path: 'users', component: UserListComponent, canActivate: [AuthGuard]},
    {path: 'users/:id', component: UserProfileComponent/*, canActivate: [AuthGuard]*/},
    {path: 'settings', component: UserSettingsComponent, canActivate: [AuthGuard]},
    {path: 'report/:id', component: ReportComponent, canActivate: [AuthGuard]},
    {path: 'pictures', component: PictureListComponent},
    {path: 'pictures/:id', component: PictureComponent},
    //{path: 'pictures/:id', component: PictureComponent},
    {path: 'create', component: CanvasComponent, canActivate: [AuthGuard]},
    {path: 'user', component: UserComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'about', component: AboutComponent},
    {path: 'admin', component: AdminPanelComponent, canActivate: [ModGuard]},
    {path: '**', redirectTo: 'home', pathMatch: 'full'},
    /*
    {path: '',runGuardsAndResolvers: 'always', canActivate:[AuthGuard],
    children:[
        {path: '', component: },
    ]},
    */
];