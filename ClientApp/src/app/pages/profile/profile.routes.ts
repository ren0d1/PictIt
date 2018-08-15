import { Routes, RouterModule } from '@angular/router';

import { ProfileComponent } from './profile.component';
import { AuthenticatorComponent } from './components/authenticator.component';

const appRoutes: Routes = [
    {
        path: 'profile',
        component: ProfileComponent
    },
    {
        path: 'authenticator',
        component: AuthenticatorComponent
    }
];

export const ProfileRouting = RouterModule.forChild(appRoutes);
