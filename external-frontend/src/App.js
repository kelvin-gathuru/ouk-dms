import { BrowserRouter, Routes, Route } from 'react-router-dom';
import LoginScreen from './pages/login';
import About from './pages/About';
import ForgotPassword from './pages/forgot_password';
import PasswordReset from './pages/passwordReset';
import SignUp from './pages/signup';
import Landing from './pages/landing';
import Dashboard from './pages/dashboard';
import CreatePetition from './pages/createPetition';
import CreatePetitionKiswahili from './pages/createPetitionKiswahili';
import DirectPetitionUpload from './pages/DirectPetitionUpload';
import HelpAndSupport from './pages/HelpAndSupport';
import Profile from './pages/Profile';
import Activation from './pages/activate';
import Layout from './components/HeaderFooter Components/Layout';
import Layout2 from './components/HeaderFooter Components/Layout2';
import ProtectedRoutes from './components/ProtectedRoutes';
import DashboardLayout from './components/Dashboard Components/DashboardLayout';
import MyPetitions from './pages/MyPetitions';
import PetitionTracker from './pages/PetitionTracker';

//Toastify
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


function App() {
  return (
    <BrowserRouter basename='/'>
      <Routes>
        <Route
          path="/"
          element={
            <Layout>
              <Landing />
            </Layout>
          }
        />

        <Route
          path="/login"
          element={
            <Layout2>
              <LoginScreen />
            </Layout2>
          }
        />

        <Route
          path="/dashboard"
          element={
            <ProtectedRoutes>
              <DashboardLayout>
                <Dashboard />
              </DashboardLayout>
            </ProtectedRoutes>
          }
        />

        <Route
          path="/dashboard/petitions"
          element={
            <ProtectedRoutes>
              <DashboardLayout>
                <MyPetitions />
              </DashboardLayout>
            </ProtectedRoutes>
          }
        />

        <Route
          path="/dashboard/upload-petition"
          element={
            <ProtectedRoutes>
              <DashboardLayout>
                <DirectPetitionUpload />
              </DashboardLayout>
            </ProtectedRoutes>
          }
        />

        <Route
          path="/about"
          element={
            <Layout>
              <About />
            </Layout>
          }
        />
        <Route
          path="/forgot"
          element={
            <Layout2>
              <ForgotPassword />
            </Layout2>
          }
        />
        <Route
          path="/reset"
          element={
            <Layout2>
              <PasswordReset />
            </Layout2>
          }
        />
        <Route
          path="/petition-tracker"
          element={
            <Layout>
              <PetitionTracker />
            </Layout>
          }
        />
        <Route
          path="/help-support"
          element={
            <ProtectedRoutes>
              <DashboardLayout>
                <HelpAndSupport />
              </DashboardLayout>
            </ProtectedRoutes>
          }
        />
        <Route
          path="/signup"
          element={
            <Layout2>
              <SignUp />
            </Layout2>
          }
        />
        <Route
          path="/activation"
          element={
            <Layout2>
              <Activation />
            </Layout2>
          }
        />
        <Route
          path="/create-petition"
          element={
            <ProtectedRoutes>
              <DashboardLayout>
                <CreatePetition />
              </DashboardLayout>
            </ProtectedRoutes>
          }
        />
        <Route
          path="/create-petition-kiswahili"
          element={
            <ProtectedRoutes>
              <DashboardLayout>
                <CreatePetitionKiswahili />
              </DashboardLayout>
            </ProtectedRoutes>
          }
        />

        <Route
          path="/profile"
          element={
            <ProtectedRoutes>
              <DashboardLayout>
                <Profile />
              </DashboardLayout>
            </ProtectedRoutes>
          }
        />
      </Routes>

      {/* Toast container for notifications */}
      <ToastContainer
        position="top-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="light"
      />
    </BrowserRouter>
  );
}

export default App;
