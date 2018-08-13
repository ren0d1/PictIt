import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login.component';

const appRoutes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    }
];

export const LoginRouting = RouterModule.forChild(appRoutes);
