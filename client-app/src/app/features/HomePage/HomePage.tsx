import { useState } from "react";
import LoginForm from "../Users/LoginForm";
import RegisterForm from "../Users/RegisterForm";
import "./HomePage.css";

export default function HomePage() {
  const [registerNow, setRegisterNow] = useState(false); // Local state

  return (
    <div className="background">
    <div className="overlay"></div>
    <div className="form-container">
      {registerNow ? <RegisterForm /> : <LoginForm />}
      
      <button 
        onClick={() => setRegisterNow(!registerNow)} 
        className="toggle-button"
      >
        {registerNow ? 'Already have an account? Login' : 'Don’t have an account? Register'}
      </button>
    </div>
  </div>
);
}

