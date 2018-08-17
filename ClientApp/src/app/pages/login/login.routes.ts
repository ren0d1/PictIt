import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login.component';
import { ForgotPasswordComponent } from './components/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password.component';
import { Login2faComponent } from './components/login-2fa.component';
import { RecoveryCodeComponent } from './components/recovery-code.component';

const appRoutes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'forgot-password',
        component: ForgotPasswordComponent
    },
    {
        path: 'reset-password',
        component: ResetPasswordComponent
    },
    {
        path: 'login-2fa',
        component: Login2faComponent
    },
    {
        path: 'recovery-code',
        component: RecoveryCodeComponent
    }
];

export const LoginRouting = RouterModule.forChild(appRoutes);
