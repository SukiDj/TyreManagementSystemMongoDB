// src/app/layout/App.tsx
import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useStore } from '../stores/store';
import { Container, Loader } from 'semantic-ui-react';
import NavBar from './NavBar';
import { Outlet } from 'react-router-dom';

export default observer(function App() {
  const {commonStore, userStore} = useStore();

  useEffect(()=>{
    if(commonStore.token){
      userStore.getUser().finally(() => commonStore.setAppLoaded())
    }else{
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore])

  if (!commonStore.appLoaded) {
    return <Loader active content='Loading app...' />;
  }

  return (
    <>
      <NavBar />
      <div className='page-content'>
        <Container>
          <Outlet />
        </Container>
      </div>
    </>
  );
});
