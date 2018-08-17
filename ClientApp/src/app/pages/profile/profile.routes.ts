import { Routes, RouterModule } from '@angular/router';

import { ProfileComponent } from './profile.component';
import { AuthenticatorComponent } from './components/authenticator.component';
import { ShowCodesComponent } from './components/show-codes.component';

const appRoutes: Routes = [
    {
        path: 'profile',
        component: ProfileComponent
    },
    {
        path: 'authenticator',
        component: AuthenticatorComponent
    },
    {
        path: 'show-codes',
        component: ShowCodesComponent
    }
];

export const ProfileRouting = RouterModule.forChild(appRoutes);
