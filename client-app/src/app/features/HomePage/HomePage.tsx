import { useState } from 'react';
import LoginForm from '../Users/LoginForm';
import RegisterForm from '../Users/RegisterForm';
import './HomePage.css';

export default function HomePage() {
  const [registerNow, setRegisterNow] = useState(false);

  return (
    <div className="auth-viewport">
      <div className="auth-card">
        <div className="brand">
          <div className="brand-icon">🛞</div>
          <div className="brand-text">
            <h1>Tyre Management</h1>
            <p>{registerNow ? 'Create an account to continue.' : 'Welcome back — sign in to continue.'}</p>
          </div>
        </div>

        <div className="auth-tabs">
          <button
            className={!registerNow ? 'tab active' : 'tab'}
            onClick={() => setRegisterNow(false)}
            type="button"
          >
            Sign in
          </button>
          <button
            className={registerNow ? 'tab active' : 'tab'}
            onClick={() => setRegisterNow(true)}
            type="button"
          >
            Register
          </button>
        </div>

        <div className="auth-body">
          {registerNow ? <RegisterForm /> : <LoginForm />}
        </div>

        <div className="auth-footer">
          <button className="link" onClick={() => setRegisterNow(!registerNow)} type="button">
            {registerNow ? 'Already have an account? Sign in' : 'Don’t have an account? Register'}
          </button>
        </div>
      </div>
    </div>
  );
}
