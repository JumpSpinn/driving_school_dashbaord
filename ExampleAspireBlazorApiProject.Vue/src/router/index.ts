import {createRouter, createWebHistory, type RouteRecordRaw} from 'vue-router'
import MainLayout from "@/layouts/MainLayout.vue";
import {RoutePage} from "@/enums/RoutePage.ts";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    component: MainLayout,
    children: [
      {
        path: "",
        name: RoutePage.DASHBOARD,
        component: () => import("@/views/DashboardView.vue")
      },
      {
        path: RoutePage.DRIVING_SCHOOL,
        name: RoutePage.DRIVING_SCHOOL,
        component: () => import("@/views/DrivingSchoolView.vue")
      },
      {
        path: RoutePage.STUDENTS,
        name: RoutePage.STUDENTS,
        component: () => import("@/views/StudentView.vue")
      },
      {
        path: RoutePage.INSTRUCTOR,
        name: RoutePage.INSTRUCTOR,
        component: () => import("@/views/InstructorView.vue")
      },
      {
        path: RoutePage.THEORY_LESSON,
        name: RoutePage.THEORY_LESSON,
        component: () => import("@/views/TheoryLessonView.vue")
      },
      {
        path: RoutePage.COURSE_BOOKING,
        name: RoutePage.COURSE_BOOKING,
        component: () => import("@/views/CourseBookingView.vue")
      }
    ]
  },
  {
    path: '/:catchAll(.*)',
    redirect: { name: RoutePage.DASHBOARD }
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
  scrollBehavior(to, from, savedPosition){
    if(savedPosition)
      return savedPosition;

    if(to.hash)
      return { el: to.hash, behavior: "smooth" }

    return { top: 0, behavior: "smooth" }
  }
})

export default router
