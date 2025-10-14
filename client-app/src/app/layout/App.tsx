import { useEffect, useState } from 'react';
import axios from 'axios';
import { Tyre } from '../models/tyre';
import NavBar from './NavBar';
import { Container } from 'semantic-ui-react';
import { Outlet } from 'react-router-dom';


function App() {
  

  return (
    <>
  
            <Outlet />
    </>
  );
}

export default App;
